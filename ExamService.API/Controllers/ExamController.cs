using Contracts;
using Contracts.Events;
using Exam.API.Application.Services;
using Exam.API.Data;
using Exam.API.Outbox;
using Exam.Contracts.Protos;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text.Json;
using static MassTransit.ValidationResultExtensions;

namespace Exam.API.Controllers
{

    [ApiController]
    [Route("api/exam")]
    public class ExamController : ControllerBase
    {
        private readonly ResultGrpc.ResultGrpcClient _resultClient;
        private readonly ExamDbContext _examDbContext;
        private readonly IExamService _examService;
        private readonly IPublishEndpoint _publishEndpoint;
        public ExamController(ResultGrpc.ResultGrpcClient resultClient, IPublishEndpoint publishEndpoint, ExamDbContext examDbContext, IExamService examService)
        {
            _resultClient = resultClient;
            _publishEndpoint = publishEndpoint;
            _examDbContext = examDbContext;
            _examService = examService;
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> Evaluate([FromBody] ExamRequest examRequest)
        {
            int score = (examRequest.CorrectAnswers * 100) / examRequest.TotalQuestions;

            await _publishEndpoint.Publish(new ExamEvaluatedEvent
            {
                StudentId = examRequest.StudentId,
                Score = score
            });

            var result = await _resultClient.GenerateResultAsync(new ResultRequest { Score = score });
            return Ok(new
            {
                score = score,
                Result = $"{result.Grade}-{result.Remark}"
            });
        }


        [HttpPost("Outboxevaluate")]
        public async Task<IActionResult> OutboxEvaluate([FromBody] ExamRequest request)
        {
            int score = (request.CorrectAnswers * 100) / request.TotalQuestions;

            var domainEvent = new ExamEvaluatedEvent
            {
                StudentId = request.StudentId,
                Score = score,
             
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = EventTypes.ExamEvaluated,
                Content = JsonSerializer.Serialize(domainEvent),
                OccurredOn = DateTime.UtcNow
            };

            _examDbContext.OutboxMessages.Add(outboxMessage);

            await _examDbContext.SaveChangesAsync();

            return Ok(new { Score = score, Message = "Saved & Event Stored" });
        }

        [HttpPost("createforDDD")]
        public async Task<IActionResult> createforDDD(string studentId)
        {
            var examId = await _examService.CreateExamAsync(studentId);

            return Ok(examId);
        }

        [HttpPost("evaluateforDDD")]
        public async Task<IActionResult> evaluateforDDD(string examId, int correct, int total)
        {
            await _examService.EvaluateExamAsync(examId, correct, total);
            return Ok();
        }
    }
}

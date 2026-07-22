using Exam.Contracts.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Exam.Contracts.Protos.ExamGrpc;

namespace Result.API.Controllers
{
  
    [ApiController]
    [Route("api/result")]
    public class ResultController : ControllerBase
    {
        private readonly ExamGrpc.ExamGrpcClient _examGrpcClient;
        public ResultController(ExamGrpcClient examGrpcClient)
        {
            _examGrpcClient = examGrpcClient;
        }

        [HttpPost("evaluate-from-exam")]
        public async Task<IActionResult> EvaluteFromExam([FromBody] ExamRequest request)
        {

            var examReply = await _examGrpcClient.EvaluateAsync(request);

            return Ok(new
            {
                examReply.Score,
                examReply.Result
            });

        }
    }
}

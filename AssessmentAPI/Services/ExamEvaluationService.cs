using Exam.Contracts.Protos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.API.Services
{
    public class ExamEvaluationService
    {
        private readonly ExamGrpc.ExamGrpcClient _client;
        public ExamEvaluationService(ExamGrpc.ExamGrpcClient client)
        {
            _client = client;
        }
        public async Task<ExamReply> EvaluateAsync(string studentId, int total, int correct)
        {
            var request = new ExamRequest
            {
                StudentId = studentId,
                TotalQuestions = total,
                CorrectAnswers = correct
            };
            //var options = new CallOptions(deadline: DateTime.UtcNow.AddSeconds(2));
            //use cancellationtoken
            return await _client.EvaluateAsync(request);
        }


        public async IAsyncEnumerable<string> EvaluateStreamAsync(string studentId, int total, int correct)
        {
            var request = new ExamRequest
            {
                StudentId = studentId,
                TotalQuestions = total,
                CorrectAnswers = correct

            };
            using var call = _client.EvaluateStream(request);
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                yield return $"{response.Percentage}%-{response.Message}";
            }
        }

        public async Task StartLiveSessionAsync(string studentId)
        {

            //var deadline = DateTime.UtcNow.AddSeconds(10);
            using var call = _client.LiveSession();
            // Task to READ responses from ExamService
            var readTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine($"Server:{response.Action}-{response.Message}");
                }
            });

            // Simulate sending live events

            var events = new[]
            {
       new LiveEvent{ StudentId = studentId,EventType ="AnswerSubmitted",Data ="Q1"},
       new LiveEvent{ StudentId = studentId,EventType ="TabSwitch",Data =""},
       new LiveEvent{ StudentId = studentId,EventType ="FaceNotDetected",Data =""}
            };

            foreach (var evt in events)
            {
                await call.RequestStream.WriteAsync(evt);
                await Task.Delay(1000);
            }
            await call.RequestStream.CompleteAsync();
            await readTask;


        }


    }
}

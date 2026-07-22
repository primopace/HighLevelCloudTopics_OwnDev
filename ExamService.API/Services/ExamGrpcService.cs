using Exam.Contracts.Protos;
using Grpc.Core;

namespace Exam.API.Services
{
    public class ExamGrpcService : ExamGrpc.ExamGrpcBase
    {
        private readonly ResultGrpc.ResultGrpcClient _resultClient;
        public ExamGrpcService(ResultGrpc.ResultGrpcClient resultClient)
        {
            _resultClient = resultClient;
        }
        public override async Task<ExamReply> Evaluate(ExamRequest examRequest, ServerCallContext serverCallContext)
        {
            var httpContext = serverCallContext.GetHttpContext();
            Console.WriteLine($"Protocol : {httpContext.Request.Protocol}");
            Console.WriteLine($"ContentType : {httpContext.Request.ContentType}");
            int score = (examRequest.CorrectAnswers * 100) / (examRequest.TotalQuestions);
            //return Task.FromResult(new ExamReply
            //{
            //    Score = score,
            //    Result = score >= 50 ? "pass" : "fail"
            //});

            //return new ExamReply
            //{
            //    Score = score,
            //    Result = score >= 50 ? "pass" : "fail"
            //};

            // gRPC → gRPC call
            var result = await _resultClient.GenerateResultAsync(
                new ResultRequest { Score = score });

            return new ExamReply
            {
                Score = score,
                Result = $"{result.Grade} - {result.Remark}"
            };
        }

        public override async Task EvaluateStream(ExamRequest request, IServerStreamWriter<ExamProgress> responseStream, ServerCallContext context)
        {

            for (int i = 1; i <= request.TotalQuestions; i++)
            {
                await Task.Delay(500);
                await responseStream.WriteAsync(new ExamProgress
                {
                    Message = $"Evaluating Question {i}",
                    Percentage = (i * 100) / request.TotalQuestions
                });
            }
            int score = (request.CorrectAnswers * 100) / request.TotalQuestions;
            await responseStream.WriteAsync(new ExamProgress
            {
                Message = $"Final Result: {(score > 50 ? "Pass" : "Fail")} (Score:{score})",
                Percentage = 100
            });

        }

        public override async Task LiveSession(IAsyncStreamReader<LiveEvent> requestStream,IServerStreamWriter<LiveResponse>responseStream,
            ServerCallContext context)
        {

            await foreach (var evt in  requestStream.ReadAllAsync())
            {
                Console.WriteLine($"Event:{evt.EventType}");
                if(evt.EventType == "TabSwitch")
                {
                    await responseStream.WriteAsync(new LiveResponse
                    {
                        Message = "Do not switch tabs!",
                        Action = "Warning"
                    });
                }
                else if (evt.EventType == "FaceNotDetected")
                {
                    await responseStream.WriteAsync(new LiveResponse
                    {
                        Message = "Face not visible!",
                        Action = "FLAG"
                    });
                }
               else  if (evt.EventType == "AnswerSubmitted")
                {
                    await responseStream.WriteAsync(new LiveResponse
                    {
                        Message = "Answer recieved!",
                        Action = "ACK"
                    });
                }

            }

        }


       


        




    }



}

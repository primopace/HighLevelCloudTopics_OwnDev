using Exam.Contracts.Protos;
using Grpc.Core;

namespace Result.API.Services
{
    public class ResultGrpcService : ResultGrpc.ResultGrpcBase
    {

        public override  Task<ResultReply> GenerateResult(ResultRequest request, ServerCallContext context)
        {
            string grade = request.Score switch
            {
                >= 80 => "A",
                >= 60 => "B",
                >= 50 => "C",
                >= 0 => "F",
                _ => throw new ArgumentOutOfRangeException()

            };
            if (request.Score < 0 || request.Score > 100)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid score"));
            return Task.FromResult(new ResultReply
            {
                Grade = grade,
                Remark = grade == "F" ? "Failed":"Passed"

            });
        }
    }
}

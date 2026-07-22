namespace Exam.API.Application.Services
{
    public interface IExamService
    {
        Task EvaluateExamAsync(string examId, int correct, int total);
        Task<Guid> CreateExamAsync(string studentId);
    }
}

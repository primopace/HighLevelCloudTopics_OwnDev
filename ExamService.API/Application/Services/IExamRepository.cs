using Exam.API.Domain;

namespace Exam.API.Application.Services
{
    public interface IExamRepository
    {
        Task<ExamModel?> GetByIdAsync(string id);

        Task AddAsync(ExamModel exam);

       
    }
}

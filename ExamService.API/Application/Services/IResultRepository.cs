using Exam.API.Models;

namespace Exam.API.Application.Services
{
    public interface IResultRepository
    {
        Task AddAsync(ExamResultModel examResultModel);
    }
}

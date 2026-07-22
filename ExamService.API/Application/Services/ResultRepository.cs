using Exam.API.Domain;
using Exam.API.Models;

namespace Exam.API.Application.Services
{
    public class ResultRepository: IResultRepository
    {
       public Task AddAsync(ExamResultModel examResultModel)
        {
            Console.WriteLine(
            $"Exam evaluated for {examResultModel.StudentId} Score:{examResultModel.Score}");

            return Task.CompletedTask;
        }
    }
}

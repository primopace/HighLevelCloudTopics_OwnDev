using Exam.API.Domain;

namespace Exam.API.Application.Services
{
    public class AuditService: IAuditService
    {
        public Task RecordAsync(string status, string studentid, string score)
        {
           Console.WriteLine(
            $"Exam evaluated for {studentid} Score:{score}");
            return Task.CompletedTask;
        }
    }
}

namespace Exam.API.Application.Services
{
    public interface IAuditService
    {

        Task RecordAsync(string status,string studentid,string Score);
    }
}

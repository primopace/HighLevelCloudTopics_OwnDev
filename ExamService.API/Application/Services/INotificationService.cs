namespace Exam.API.Application.Services
{
    public interface INotificationService
    {
        Task SendAsync(string studentId,string score);
    }
}

using Exam.API.Domain;

namespace Exam.API.Application.Services
{
    public class NotificationService: INotificationService
    {

       public Task SendAsync(string studentId, string score)
        {
            Console.WriteLine(
            $"Exam evaluated for {studentId} Score:{score}");
            return Task.CompletedTask;

        }
    }
}

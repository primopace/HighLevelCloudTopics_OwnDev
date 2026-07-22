using Exam.API.Application.Services;

namespace Exam.API.Domain.Handlers
{
    public class NotifyStudentExamEvaluatedHandler
     : IDomainEventHandler<ExamEvaluatedDomainEvent>
    {
        private readonly INotificationService _notificationService;

        public NotifyStudentExamEvaluatedHandler(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(ExamEvaluatedDomainEvent domainEvent)
        {
            await _notificationService.SendAsync(
                domainEvent.StudentId,
                $"Your exam score is {domainEvent.Score}"
            );
        }
    }
}

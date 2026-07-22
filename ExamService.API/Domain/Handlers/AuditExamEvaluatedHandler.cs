using Exam.API.Application.Services;

namespace Exam.API.Domain.Handlers
{
   


    public class AuditExamEvaluatedHandler
    : IDomainEventHandler<ExamEvaluatedDomainEvent>
    {
        private readonly IAuditService _audit;

        public AuditExamEvaluatedHandler(IAuditService audit)
        {
            _audit = audit;
        }

        public async Task Handle(ExamEvaluatedDomainEvent domainEvent)
        {
            await _audit.RecordAsync(
                "ExamEvaluated",
                domainEvent.StudentId,
                $"Score:{domainEvent.Score}");
        }
    }
}

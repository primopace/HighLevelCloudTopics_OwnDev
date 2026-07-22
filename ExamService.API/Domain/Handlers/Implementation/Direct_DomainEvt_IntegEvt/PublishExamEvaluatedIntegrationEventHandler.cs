using Contracts.Events;
using MassTransit;

namespace Exam.API.Domain.Handlers.Implementation.Direct_DomainEvt_IntegEvt
{
    public class PublishExamEvaluatedIntegrationEventHandler
    : IDomainEventHandler<ExamEvaluatedDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublishExamEvaluatedIntegrationEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(ExamEvaluatedDomainEvent domainEvent)
        {
            //await _publishEndpoint.Publish(new ExamEvaluatedEvent
            //{
            //    StudentId = domainEvent.StudentId,
            //    Score = domainEvent.Score
            //});
            await Task.CompletedTask;
        }
    }
}

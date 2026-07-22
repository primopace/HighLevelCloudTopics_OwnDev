using Contracts.Events;
using Exam.API.Application.Services;
using Exam.API.Models;

namespace Exam.API.Domain.Handlers.Implementation
{
    public class SaveExamResultHandler:IDomainEventHandler<ExamEvaluatedDomainEvent>
    {
        private readonly IResultRepository _repository;
        public SaveExamResultHandler(IResultRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(ExamEvaluatedDomainEvent domainEvent)
        {

            var result = new ExamResultModel
            {
                StudentId = domainEvent.StudentId,
                Score = domainEvent.Score,
               // EvaluatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(result);


        }
    }
}

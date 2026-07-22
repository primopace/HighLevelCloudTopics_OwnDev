using Contracts.Events;
using MassTransit;

namespace Result.API.Consumers
{
    public class ExamEvaluatedConsumer:IConsumer<ExamEvaluatedEvent>
    {

        public async Task Consume(ConsumeContext<ExamEvaluatedEvent> context)
        {
            var score = context.Message.Score;
            string grade = score switch

            {
                >= 80 => "A",
                >=60=>"B",
                >=50=>"C",
                _=>"F"
            };

            Console.WriteLine($"Student:{context.Message.StudentId}");
            Console.WriteLine($"Grade:{ grade}");

            await Task.CompletedTask;
        }
    }
}

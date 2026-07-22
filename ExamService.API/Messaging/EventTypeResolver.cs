using Contracts;
using Contracts.Events;

namespace Exam.API.Messaging
{
    public static class EventTypeResolver
    {
        public static readonly Dictionary<string, Type> EventMap = new()
    {
        { EventTypes.ExamEvaluated, typeof(ExamEvaluatedEvent) }
    };
    }
}

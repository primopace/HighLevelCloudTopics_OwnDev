    namespace Exam.API.Domain
{
    public class ExamEvaluatedDomainEvent: DomainEvent
    {
        public string StudentId { get; }
        public int Score { get; }
        public ExamEvaluatedDomainEvent(string studendId,int score)
        {
            StudentId = studendId;
            Score = score;


        }

    }
}

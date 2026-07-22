namespace Exam.API.Domain
{
    public class ExamModel : Entity
    {
        public string StudentId { get; private set; } = null!;
        public int Score { get; private set; }
        //C# compiler expects non-nullable properties to be initialized inside the constructor
        //Score is value type so the default value is zero for int
        private ExamModel() { } // Required for EF Core

        public ExamModel(string studentId)
        {
            Id = Guid.NewGuid();
            StudentId = studentId;
        }
        public void Evaluate(int correct, int total)
        {
            if (total == 0)
                throw new ArgumentException("Total questions cannot be zero");
            Score = (correct * 100) / total;
            AddDomainEvent(new ExamEvaluatedDomainEvent(StudentId, Score));
        }
    }
}

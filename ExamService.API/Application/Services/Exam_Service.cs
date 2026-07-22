using Exam.API.Domain;

namespace Exam.API.Application.Services
{
    public class Exam_Service: IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IUnitOfWork _unitOfWork;
        public Exam_Service(IExamRepository examRepository, IUnitOfWork unitOfWork)
        {
            _examRepository = examRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task EvaluateExamAsync(string examId, int correct, int total)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null)
                throw new Exception("Exam not found");

            exam.Evaluate(correct, total);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Guid> CreateExamAsync(string studentId)
        {
            var exam = new ExamModel(studentId);

            await _examRepository.AddAsync(exam);

            await _unitOfWork.SaveChangesAsync();

            return exam.Id;
        }
    }
}

using Exam.API.Data;
using Exam.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Exam.API.Application.Services
{
    public class ExamRepository : IExamRepository
    {
        private readonly ExamDbContext _context;

        public ExamRepository(ExamDbContext context)
        {
            _context = context;
        }

     

        public async Task<ExamModel?> GetByIdAsync(string id)
        {
            return await _context.Exams.FirstOrDefaultAsync(x => x.StudentId == id);
        }

        public async Task AddAsync(ExamModel exam)
        {
            await _context.Exams.AddAsync(exam);
        }
    }
}

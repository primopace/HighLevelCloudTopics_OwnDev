using Assessment.Domain.Common;
using Assessment.Domain.Exam;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.Application.Services
{
    public class ExaminationService
    {
        public void StartExam(UserContext user)
        {
            if (!user.HasScope("exam.start"))
                throw new UnauthorizedAccessException();

            if (!user.IsWithinExamWindow())
              throw new BusinessException(
                "Exam window closed",
                "EXAM_WINDOW_CLOSED"
            );
        }
    }
}

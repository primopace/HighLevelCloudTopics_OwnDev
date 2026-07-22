using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Assessment.Domain.Exam
{
    public class UserContext
    {
        public string UserId { get; init; }
        public IReadOnlyList<string> Scopes { get; init; }
        public DateTime ExamStart { get; init; }
        public DateTime ExamEnd { get; init; }

        public bool HasScope(string scope)
            => Scopes.Contains(scope);

        public bool IsWithinExamWindow()
            => DateTime.UtcNow >= ExamStart &&
               DateTime.UtcNow <= ExamEnd;

        public static UserContext FromClaims(ClaimsPrincipal user)
        {
            return new UserContext
            {
                UserId = user.FindFirst("sub")!.Value,
                Scopes = user.FindAll("scope").Select(c => c.Value).ToList(),
                ExamStart = DateTime.Parse(
                    user.FindFirst("exam_start")!.Value),
                ExamEnd = DateTime.Parse(
                    user.FindFirst("exam_end")!.Value)
            };
        }
    }

}

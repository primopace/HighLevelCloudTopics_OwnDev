using Microsoft.AspNetCore.Authorization;

namespace AssessmentAPI.Requirements
{
    public class ExamWindowRequirement : IAuthorizationRequirement { }


    public class ExamWindowHandler
    : AuthorizationHandler<ExamWindowRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ExamWindowRequirement requirement)
        {
            var now = DateTime.UtcNow;

            if (now.Hour >= 9 && now.Hour <= 12)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}

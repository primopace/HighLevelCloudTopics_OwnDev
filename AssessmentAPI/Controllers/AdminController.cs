using Assessment.API.Services;
using Assessment.Application.Services;
using Assessment.Domain.Exam;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentAPI.Controllers
{

    [ApiController]
    [Route("api")]
    public class AdminController : ControllerBase
    {

        private readonly RealtimeNotificationService _notify;
        private readonly ExaminationService _examService;
        public AdminController(ExaminationService examService, RealtimeNotificationService notify)
        {
            _examService = examService;
            _notify = notify;
        }
        [Authorize(Policy = "Admin.Access")]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
               => Ok("Admin access granted");

        [Authorize(Policy = "Assessment.Read")]
        [HttpGet("read")]
        public IActionResult ReadData()
            => Ok("Read access granted");

        [Authorize(Policy = "Assessment.Write")]
        [HttpPost("write")]
        public IActionResult WriteData()
            => Ok("Write access granted");

        [Authorize(Policy = "Exam.Start")]
        [HttpPost("exam/start")]
        public IActionResult StartExam()
        {
            var userContext = UserContext.FromClaims(User);

            _examService.StartExam(userContext);
            return Ok("Exam started");
        }



        [Authorize(Policy = "Exam.Window")]
        [HttpPost("Precheck/exam/start")]
        public IActionResult PrecheckStartExam()
         => Ok("Exam started");
        [Authorize]
        [HttpGet("claims")]
        public IActionResult Claims()
            => Ok(User.Claims.Select(c => new { c.Type, c.Value }));

        [HttpPost("flag/{userId}")]
        public async Task<IActionResult> FlagCandidate(string userId)
        {
            await _notify.SendToUserAsync(userId, $"⚠ Suspicious activity detected for {userId}");
            return Ok();

        }

        [HttpPost("announcement")]
       public async Task<IActionResult> Announcement()
        {
            await _notify.BroadcastAsync("Exam will end in 5 minutes");
            return Ok();
        }

            

    }

}





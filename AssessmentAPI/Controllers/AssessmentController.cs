using Assessment.API.Services;
using Assessment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentAPI.Controllers
{

    //[Authorize]
    [ApiController]
    [Route("api/Assessment")]
    public class AssessmentController : ControllerBase
    {
        private readonly ExamEvaluationService _examEvaluationService;
        public AssessmentController(ExamEvaluationService examEvaluationService)
        {
            _examEvaluationService = examEvaluationService;
        }
        [HttpGet("checkprotected")]
        public IActionResult Get() => Ok("Protected data");


        [HttpGet("evaluate")]
        public async Task<IActionResult> Evaluate()
        {

            var result = await _examEvaluationService.EvaluateAsync("student1", 10, 7);
            return Ok(result);

        }


        [HttpGet("evaluate-stream")]
        public async Task EvaluateStream()
        {
            Response.ContentType = "text/plain";
            await foreach (var message in _examEvaluationService.EvaluateStreamAsync("student1", 10, 7))
            {
                await Response.WriteAsync(message + "\n");
                await Response.Body.FlushAsync(); //Push Immediately
            }
        }



        [HttpGet("live-session")]
        public async Task<IActionResult> StartLive()
        {
            await _examEvaluationService.StartLiveSessionAsync("student1");
            return Ok("Live session Completed");
        }


    }

}

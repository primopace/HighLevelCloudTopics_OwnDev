using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
  
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public IActionResult Token()
        {
            return Ok("IdentityServer handles token endpoint internally");
        }
    }
}

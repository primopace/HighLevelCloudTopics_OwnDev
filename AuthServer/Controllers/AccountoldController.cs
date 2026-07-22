using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServer.Controllers
{
    [Route("accountold")]
    public class AccountoldController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public AccountoldController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        // ----------------------------
        // GET: /account/login
        // ----------------------------
        [HttpGet("Mlogin")]
        public IActionResult Login(string returnUrl)
        {
            return Content($@"
                <html>
                <body>
                    <h2>Login</h2>
                    <form method='post' action='/account/login'>
                        <input type='hidden' name='returnUrl' value='{returnUrl}' />
                        <input name='username' placeholder='username' />
                        <br/>
                        <input name='password' type='password' placeholder='password' />
                        <br/>
                        <button type='submit'>Login</button>
                    </form>
                </body>
                </html>",
                "text/html");
        }

        // ----------------------------
        // POST: /account/login
        // ----------------------------
        [HttpPost("Mlogin")]
        public async Task<IActionResult> LoginPost(
            string username,
            string password,
            string returnUrl)
        {
            // 🚨 TEMP USER (learning)
            if (username != "test" || password != "1234")
                return Unauthorized("Invalid credentials");

            var userClaims = new List<Claim>
            {
                new Claim("sub", "1"),
                new Claim("name", username),
                new Claim("role", "Admin"),
                new Claim("role", "Candidate")
            };

            var identity = new ClaimsIdentity(
                userClaims,
                "password"
            );

            var principal = new ClaimsPrincipal(identity);

            // 🔐 Create auth cookie
            await HttpContext.SignInAsync(principal);

            // 🚀 Resume authorization flow
            if (_interaction.IsValidReturnUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("~/");
        }

        // ----------------------------
        // POST: /account/logout
        // ----------------------------
        [HttpPost("Mlogout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await HttpContext.SignOutAsync();

            var context = await _interaction.GetLogoutContextAsync(logoutId);

            if (context?.PostLogoutRedirectUri != null)
                return Redirect(context.PostLogoutRedirectUri);

            return Redirect("~/");
        }
    }
}


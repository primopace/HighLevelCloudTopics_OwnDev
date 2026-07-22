using AuthServer.Identity;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServer.Controllers
{


    //configure external login for google
    [Route("External")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _interaction = interaction;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("Challenge")]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("Callback"),
                Items =
            {
                { "returnUrl", returnUrl }
            }
            };

            return Challenge(props, provider);
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(
                IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var externalUser = result.Principal;

            var email = externalUser.FindFirst(ClaimTypes.Email)?.Value;
            var provider = result.Properties.Items[".AuthScheme"];

            // 🔹 Find or create local user
            var user = await _userManager.FindByEmailAsync(email!);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user,
                    new UserLoginInfo(provider!, email!, provider));
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            await HttpContext.SignOutAsync(
                IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var returnUrl = result.Properties.Items["returnUrl"];
            return Redirect(returnUrl!);
        }
    }
}

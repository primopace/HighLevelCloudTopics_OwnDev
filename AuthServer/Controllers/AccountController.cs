using AuthServer.Identity;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{


    [Route("account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IPersistedGrantService _persistedGrantService;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction,
            IPersistedGrantService persistedGrantService    )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _persistedGrantService = persistedGrantService;
        }

        // ----------------------------
        // GET: google login page
        // ----------------------------
        [HttpGet("login")]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            return Content($@"
<html>
<head>
    <title>Login</title>
</head>
<body>
    <h2>Login</h2>

    <a href=""/External/Challenge?provider=Google&returnUrl={Uri.EscapeDataString(returnUrl)}"">
        Login with Google
    </a>

    <br/><br/>

    <form method=""post"" action=""/account/login"">
        <input name=""username"" placeholder=""Username"" />
        <input name=""password"" type=""password"" placeholder=""Password"" />
        <input type=""hidden"" name=""returnUrl"" value=""{returnUrl}"" />
        <button type=""submit"">Login</button>
    </form>

</body>
</html>
", "text/html");
        }



        // ----------------------------
        // GET: /account/login    for identityserver's login page
        // ----------------------------
//        [HttpGet("login")]
//        public IActionResult Login(string returnUrl)
//        {
//            return Content($@"
//<html>
//<body>
//    <h2>Login</h2>
//    <form method='post' action='/account/login'>
//        <input type='hidden' name='returnUrl' value='{returnUrl}' />
//        <input name='username' placeholder='username' />
//        <br/>
//        <input name='password' type='password' placeholder='password' />
//        <br/>
//        <button type='submit'>Login</button>
//    </form>
//</body>
//</html>", "text/html");
//        }

        // ----------------------------
        // POST: /account/login
        // ----------------------------
        [HttpPost("login")]
        public async Task<IActionResult> LoginPost(
            string username,
            string password,
            string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            // 🔥 THIS resumes the IdentityServer flow
            if (_interaction.IsValidReturnUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("~/");
        }

        // ----------------------------
        // POST: /account/logout
        // ----------------------------
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            // 2️⃣ Revoke all persisted grants (refresh tokens, codes)
            var subjectId = User.FindFirst("sub")?.Value;

            if (!string.IsNullOrEmpty(subjectId))
            {
                await _persistedGrantService.RemoveAllGrantsAsync(
                    subjectId,
                    clientId: "assessment_spa"
                );
            }
            //Revoke ALL client sessions(optional)
            // await _persistedGrantService.RemoveAllGrantsAsync(subjectId);
            //await _persistedGrantService.RemoveAllGrantsAsync(user.Id);
            var context = await _interaction.GetLogoutContextAsync(logoutId);

            if (context?.PostLogoutRedirectUri != null)
                return Redirect(context.PostLogoutRedirectUri);

            return Redirect("~/");
        }

     

        //[HttpGet("logout")]
        //public async Task<IActionResult> Logout(string logoutId)
        //{
        //    var context = await _interaction.GetLogoutContextAsync(logoutId);

        //    // no UI → auto-logout
        //    await _signInManager.SignOutAsync();

        //    if (context?.PostLogoutRedirectUri != null)
        //        return Redirect(context.PostLogoutRedirectUri);

        //    return Redirect("~/");
        //}


    }
}

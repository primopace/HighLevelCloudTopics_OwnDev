using AuthServer.Identity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            if (user == null) return;
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new Claim("user_id", user.Id),
            new Claim("email", user.Email)
        };
         
            claims.AddRange(roles.Select(r => new Claim("role", r)));
            // Optional: domain-specific claims
            if (roles.Contains("Proctor"))
                claims.Add(new Claim("can_monitor_exam", "true"));
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null && user.IsActive;
        }
    }

}

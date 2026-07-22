using Microsoft.AspNetCore.Identity;

namespace AuthServer.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Optional (enterprise-friendly extensions)
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

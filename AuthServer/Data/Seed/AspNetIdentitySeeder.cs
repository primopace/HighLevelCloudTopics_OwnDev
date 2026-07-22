using AuthServer.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Data.Seed
{
    public static class AspNetIdentitySeeder
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            // ---- Roles ----
            string[] roles = { "Admin", "Candidate", "Proctor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ---- Admin user ----
            var admin = await userManager.FindByNameAsync("admin");

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@test.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    FullName = "System Admin"
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }

}

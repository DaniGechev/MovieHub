using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MovieHub.Data;
using MovieHub.Data.Models;

namespace MovieHub.Web.Infrastructure
{
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// Applies any pending EF Core migrations and seeds the administrator
        /// role + user. Called once at application startup from Program.cs.
        /// </summary>
        public static async Task<WebApplication> PrepareDatabaseAsync(this WebApplication app)
        {
            // Services are scoped, so we open a short-lived scope to resolve them.
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();

            // Creates the database (if needed) and applies all migrations.
            await context.Database.MigrateAsync();

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedAdministratorAsync(roleManager, userManager);

            return app;
        }

        private static async Task SeedAdministratorAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            // If the role already exists we assume seeding has run before.
            if (await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName))
            {
                return;
            }

            await roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdministratorRoleName));

            var admin = new ApplicationUser
            {
                UserName = GlobalConstants.AdminEmail,
                Email = GlobalConstants.AdminEmail,
                EmailConfirmed = true,
                NickName = "Administrator",
            };

            var result = await userManager.CreateAsync(admin, GlobalConstants.AdminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}

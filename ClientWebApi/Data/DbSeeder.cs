using ClientWebApi.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ClientWebApi.Data
{
    public class DbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> identityRole)
        {
            _userManager = userManager;
            _roleManager = identityRole;
        }
        public async Task SeedAsync()
        {
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin@123";

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    FullName = "Admin",
                    Email = adminEmail,
                    PhoneNumber = "9182882818",
                    DateOfBirth = new DateTime(1990, 1, 1),
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception("Failed to create the admin user");
                }
            }
        }
    }
}

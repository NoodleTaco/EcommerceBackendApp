using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserService.Core.models;

namespace UserService.API.helpers
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string email = "admin@example.com";
            string password = "AdminPassword123!";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new UserModel
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
    }
}
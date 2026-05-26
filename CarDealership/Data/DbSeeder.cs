using CarDealership.Models;
using Microsoft.AspNetCore.Identity;

namespace CarDealership.Data;

public static class DbSeeder
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        var adminEmail = "admin@cardealership.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrator Sistem",
                EmailConfirmed = true
            };

            var createAdminResult = await userManager.CreateAsync(newAdmin, "Admin123!");
            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}

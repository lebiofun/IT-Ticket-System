using BlazorApp1.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Data;

public static class FillDB
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // ===== Create roles =====
        string[] roles = { "EndUser", "TechSupport", "SupportManager" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // ===== Admin user (SupportManager) =====
        await EnsureUserAsync(userManager, "admin@local", "Admin123!", "SupportManager", "IT");

        // ===== Tech Support user =====
        await EnsureUserAsync(userManager, "tech@local", "Tech123!", "TechSupport", "IT Support");

        await EnsureUserAsync(userManager, "tech@local1", "Tech1234!", "TechSupport", "IT Support");


        // ===== End User =====
        await EnsureUserAsync(userManager, "user@local", "User123!", "EndUser", "Sales");
        await EnsureUserAsync(userManager, "user@local1", "User1234!", "EndUser", "Sales");

    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, string email, string password, string role, string department)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Department = department
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, role);
        }
    }
}

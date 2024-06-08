using Microsoft.AspNetCore.Identity;
using TaskTracker.Data.Entities;
using static TaskTracker.Common.AppConstant;

namespace TaskTracker.Web.Extensions;

public static class SeedExtensions
{
    public static async Task SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();

        await SeedRoles(roleManager);
        await SeedDefaultUser(scope.ServiceProvider);
    }

    private static async Task SeedRoles(RoleManager<RoleEntity> roleManager)
    {
        foreach (var roleField in Roles.All)
        {
            var role = new RoleEntity()
            {
                Name = roleField
            };

            var isRoleExist = await roleManager.RoleExistsAsync(role.Name);

            if (!isRoleExist)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedDefaultUser(IServiceProvider serviceProvider)
    {
        var userEmail = "user@gmail.com";
        var userPassword = "User@123";

        var userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        var user = await userManager.FindByEmailAsync(userEmail);

        if (user == null)
        {
            var newUser = new UserEntity
            {
                Email = userEmail,
                UserName = userEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(newUser, userPassword);

            await userManager.AddToRoleAsync(newUser, Roles.User);
        }
    }
}

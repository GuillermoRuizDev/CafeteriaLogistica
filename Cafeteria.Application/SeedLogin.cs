using Cafeteria.Application.StaticClass;
using Cafeteria.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cafeteria.Application;

public static class SeedLogin
{
    public static async Task InitializeAsync(
        IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var userManager = (UserManager<ApplicationUser>)scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await AddUsersAndRolesAsync(userManager, roleManager);
        }
    }

    private static async Task AddUsersAndRolesAsync(
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager)
    {

        //if (!await _roleManager.RoleExistsAsync(Roles.SupervisorRole))
        //    await _roleManager.CreateAsync(new IdentityRole(Roles.SupervisorRole));

        //if (!await _roleManager.RoleExistsAsync(Roles.UserRole))
        //    await _roleManager.CreateAsync(new IdentityRole(Roles.UserRole));


        var roles = new[] { Roles.UserRole, Roles.SupervisorRole, Roles.AdministratorRole };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var userCommon = await _userManager.FindByEmailAsync("usuario@usuarioprueba.net");

        if (userCommon == null)
        {
            userCommon = new ApplicationUser()
            {
                UserName = "usuario@usuarioprueba.net",
                Email = "usuario@usuarioprueba.net",
                UserId = 1,
            };
            await _userManager.CreateAsync(userCommon, "ClaveSecreta123$");
            await _userManager.AddToRoleAsync(userCommon, Roles.UserRole);
        }

        var supervisorCommon = await _userManager.FindByEmailAsync("supervisor@supervisorprueba.net");

        if (supervisorCommon == null)
        {
            supervisorCommon = new ApplicationUser()
            {
                UserName = "supervisor@supervisorprueba.net",
                Email = "supervisor@supervisorprueba.net",
                UserId = 2,
            };
            await _userManager.CreateAsync(supervisorCommon, "ClaveSecreta123$");
            await _userManager.AddToRoleAsync(supervisorCommon, Roles.SupervisorRole);
        }


    }
}
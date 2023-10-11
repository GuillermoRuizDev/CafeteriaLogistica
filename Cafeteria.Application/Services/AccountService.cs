using Cafeteria.Application.Interfaces;
using Cafeteria.Domain.Model;
using Cafeteria.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Cafeteria.Application.Services;

public class AccountService : IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    //public async Task<List<string>> Login(LoginViewModel model)
    //{
    //    var user = await _userManager.FindByEmailAsync(model.Email);

    //    if (user != null)
    //    {
    //        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

    //        if (result.Succeeded)
    //        {
    //            return (await _userManager.GetRolesAsync(user)).ToList();
    //        }
    //    }

    //    return default;
    //}

    public async Task<(SignInResult, List<string>)> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var roles = new List<string>();
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            roles = (await _userManager.GetRolesAsync(user)).ToList();
            return (result, roles);
        }

        return default;
    }

    public async Task<(IdentityResult, List<string>)> Register(RegisterViewModel model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        var roles = new List<string>();
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, model.Role.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);

            roles = (await _userManager.GetRolesAsync(user)).ToList();
        }

        return (result, roles);
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

}
using Cafeteria.Application.Interfaces;
using Cafeteria.Domain.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

    public async Task<(SignInResult, ApplicationUser)> Login(LoginViewDto model, HttpContext httpContext)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var roles = new List<string>();
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            roles = (await _userManager.GetRolesAsync(user)).ToList();

            Task.WaitAll(GenerateClaimsAsync(roles, user, httpContext));

            return (result, user);
        }

        return default;
    }
    private async Task GenerateClaimsAsync(List<string> roles, ApplicationUser user, HttpContext httpContext)
    {
        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("email", user.Email),
                };

        for (int i = 0; i < roles.Count; i++)
        {
            claims.Add(new Claim(ClaimTypes.Role, roles[i]));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, roles[i]));
        }


        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public async Task<(IdentityResult, ApplicationUser)> Register(RegisterViewDto model, HttpContext httpContext)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        var roles = new List<string>();
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, model.Role.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);

            roles = (await _userManager.GetRolesAsync(user)).ToList();

            Task.WaitAll(GenerateClaimsAsync(roles, user, httpContext));
        }

        return (result, user);
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string rol)
    {
        return await _userManager.IsInRoleAsync(user, rol);
    }

}
public record LoginViewDto(
     string Email,
     string Password,
     bool RememberMe
);
public record RegisterViewDto(
     string Email,
     string Password,
     string ConfirmPassword,
     UserRole Role
);

public enum UserRole
{
    User,
    Supervisor
}


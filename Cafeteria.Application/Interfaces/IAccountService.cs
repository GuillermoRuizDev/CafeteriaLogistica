using Cafeteria.Application.Services;
using Cafeteria.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Cafeteria.Application.Interfaces;

public interface IAccountService
{
    Task<(SignInResult, ApplicationUser)> Login(LoginViewDto model, HttpContext httpContext);
    Task<(IdentityResult, ApplicationUser)> Register(RegisterViewDto model, HttpContext httpContext);
    Task Logout();
    Task<bool> IsInRoleAsync(ApplicationUser user, string rol);
}

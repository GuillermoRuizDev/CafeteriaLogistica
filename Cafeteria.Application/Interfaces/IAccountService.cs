using Cafeteria.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Cafeteria.Application.Interfaces;

public interface IAccountService
{
    Task<(SignInResult, List<string>)> Login(LoginViewModel model);
    Task<(IdentityResult, List<string>)> Register(RegisterViewModel model);
    Task Logout();
}

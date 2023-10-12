using Cafeteria.Application.Interfaces;
using Cafeteria.Application.Services;
using Cafeteria.Application.StaticClass;
using Cafeteria.Domain.Model;
using Cafeteria.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var (result, user) = await _accountService.Login(new LoginViewDto(
                Email: model.Email,
                Password: model.Password,
                RememberMe: model.RememberMe), HttpContext);

            if (result.Succeeded)
            {
                return await RedirectLogin(user);
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrecta");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var (result, user) = await _accountService.Register(new RegisterViewDto(Email: model.Email, Password: model.Password, ConfirmPassword: model.ConfirmPassword, Role: model.Role), HttpContext);

            if (result.Succeeded)
                await RedirectLogin(user);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    private async Task<IActionResult> RedirectLogin(ApplicationUser user)
    {
        var rol = await _accountService.IsInRoleAsync(user, Roles.SupervisorRole);
        if (rol)
            return RedirectToAction("ListOrders", "WorkOrder");

        return RedirectToAction("Create", "WorkOrder");
    }
}

using Cafeteria.Application.Interfaces;
using Cafeteria.Application.StaticClass;
using Cafeteria.Domain.Model;
using Cafeteria.Domain.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cafeteria.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
    {
        _accountService = accountService;
        _signInManager = signInManager;
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
            var (result, roles) = await _accountService.Login(model);

            if (result.Succeeded)
            {
                await GenerateClaims(roles, model.Email);
                return await RedirectLogin();
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrecta");
        }

        return View(model);
    }

    private async Task<IActionResult> RedirectLogin()
    {
        if (User.IsInRole(Roles.SupervisorRole))
            return RedirectToAction("ListOrders", "WorkOrder");

        return RedirectToAction("Create", "WorkOrder");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
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
            var (result, roles) = await _accountService.Register(model);

            if (result.Succeeded)
                await RedirectLogin();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    private async Task GenerateClaims(List<string> roles, string email)
    {
        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, email),
                    //new Claim(ClaimTypes.Role, roles[0])
                };

        for (int i = 0; i < roles.Count; i++)
            claims.Add(new Claim(ClaimTypes.Role, roles[i]));

        var userIdentity = new ClaimsIdentity(claims, "login");
        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

        await HttpContext.SignInAsync(principal);
    }

}

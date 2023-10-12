using Cafeteria.Application.StaticClass;
using Cafeteria.Domain.Model;
using Cafeteria.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Cafeteria.Web;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddWebApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
                        .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Roles.UserRole, policy => policy.RequireRole(Roles.UserRole));
            options.AddPolicy(Roles.SupervisorRole, policy => policy.RequireRole(Roles.SupervisorRole));
            options.AddPolicy(Roles.AdministratorRole, policy => policy.RequireRole(Roles.AdministratorRole));
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.LoginPath = "/Account/Login";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                option.AccessDeniedPath = "/Home/Privacy";
            });

        return builder;
    }

    public static IApplicationBuilder AddWebServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        return app;
    }
}
using Cafeteria.Application.Interfaces;
using Cafeteria.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cafeteria.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
    this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IWorkOrderService, WorkOrderService>();

        return services;
    }

}

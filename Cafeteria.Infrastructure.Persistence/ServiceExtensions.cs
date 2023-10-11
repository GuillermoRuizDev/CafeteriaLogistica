using Cafeteria.Domain.Interfaces;
using Cafeteria.Infrastructure.Persistence.Context;
using Cafeteria.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cafeteria.Infrastructure.Persistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"));
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

        return services;
    }
}

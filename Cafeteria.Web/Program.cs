using Cafeteria.Application;
using Cafeteria.Infrastructure.Persistence;
using Cafeteria.Web;

var builder = WebApplication.CreateBuilder(args);
{
    builder.AddWebApplication();
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices();
}


var app = builder.Build();
{
    app.AddWebServices();
    SeedLogin.InitializeAsync(app.Services).Wait();
    SeedData.InitializeAsync(app.Services).Wait();
    app.Run();
}
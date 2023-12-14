
using NLog;
using NLog.Web;
using sidecar_lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using VaultSharp;
using OrderAPI.OrderRepo;
using OrderAPI.Services;
using OrderAPI.Infrastructure;
using OrderAPI;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    AuthSidecar sidecar = new(logger);

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddHostedService<OrderWorker>();
    builder.Services.AddSingleton<IVaultClient>(sidecar.vaultClient);
    builder.Services.AddSingleton<IInfraRepo, InfraRepo>();
    builder.Services.AddSingleton<IOrderRepo, OrderRepoMongo>();
    builder.Services.AddSingleton<IServiceOrder, ServiceOrder>();


    builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = sidecar.GetTokenValidationParameters();
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwagger("OrderAPI");

    builder.Services.AddHostedService<OrderWorker>();
    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "Your Microservice API V1");
    });

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}


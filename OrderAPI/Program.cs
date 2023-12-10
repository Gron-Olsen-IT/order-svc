
using NLog;
using NLog.Web;
using sidecar_lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using VaultSharp;
using OrderAPI.OrderRepo;
using OrderAPI.Services;
using OrderAPI.InfraRepo;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    AuthSidecar sidecar = new(logger);

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddSingleton<IVaultClient>(sidecar.vaultClient);
    builder.Services.AddScoped<IOrderRepo, OrderRepoMongo>();
    builder.Services.AddScoped<IInfraRepo, InfraRepo>();
    builder.Services.AddScoped<IServiceOrder, ServiceOrder>();

    builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = sidecar.GetTokenValidationParameters();
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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


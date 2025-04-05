using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VistaLOS.Application.Api.Extensions;
using VistaLOS.Application.Api.Middlewares;
using VistaLOS.Application.Api.Options;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Data.Master.Options;
using VistaLOS.Application.Identity.Data.Options;
using VistaLOS.Application.Identity.Services.Extensions;
using VistaLOS.Application.Identity.Services.Options;
using VistaLOS.Application.Jobs.Options;
using VistaLOS.Application.Services.Extensions;
using VistaLOS.Application.Services.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ValidateScopesOnBuild();

var configuration = builder.Configuration;

var corsPolicyOptions = configuration.GetRequiredOption<CorsPolicyOptions>(CorsPolicyOptions.Section);
var adminAuthOptions = configuration.GetRequiredOption<AdminAuthOptions>(AdminAuthOptions.Section);
var authOptions = configuration.GetRequiredOption<AuthOptions>(AuthOptions.Section);
var hangfireOptions = configuration.GetRequiredOption<HangfireOptions>(HangfireOptions.Section);
var masterDbOptions = configuration.GetRequiredOption<MasterDbOptions>(MasterDbOptions.Section);
var identityDbOptions = configuration.GetRequiredOption<IdentityDbOptions>(IdentityDbOptions.Section);
var redisOptions = configuration.GetRequiredOption<RedisOptions>(RedisOptions.Section);
var loggingOptions = configuration.GetRequiredOption<LoggingConfigOptions>(LoggingConfigOptions.Section);

builder.Services.AddSingleton(adminAuthOptions);
builder.Services.AddSingleton(authOptions);
builder.Services.AddSingleton(hangfireOptions);
builder.Services.AddSingleton(masterDbOptions);
builder.Services.AddSingleton(identityDbOptions);
builder.Services.AddSingleton(redisOptions);
builder.Services.AddSingleton(loggingOptions);

builder.Services.AddOpenTelemetry(configuration);

builder.Services.AddHealthChecks()
                .AddSqlHealthCheck()
                .AddRedisHealthCheck();

builder.AddLogging(configuration);
builder.Services.AddControllers()
                .AddControllersAsServices()
                .AddNewtonsoftJson(options => {
                    var settings = options.SerializerSettings;
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerLayer();
builder.Services.AddHostedServices();
builder.Services.AddCorsPolicy(corsPolicyOptions);
builder.Services.AddMiddlewares();
builder.Services.AddMappingProfiles();
builder.Services.AddDbContexts(configuration);
builder.Services.AddRepositories();
builder.Services.AddRedis(redisOptions);
builder.Services.AddMultitenancy();
builder.Services.AddApplicationServicesLayer();
builder.Services.AddIdentityServiceLayer();
builder.Services.AddIdentityAuthLayer(configuration);

var app = builder.Build();

app.EnsureEnvironmentNameIsCorrect();

if (!app.Environment.IsProduction()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseCors(corsPolicyOptions.PolicyName);

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<TenantResolverMiddleware>();

//app.UseMiddleware<RateLimitMiddleware>();

app.MapControllers();

app.UseHealthChecks("/health",
    new HealthCheckOptions {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();

public partial class Program { }
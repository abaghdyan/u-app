using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewRelic.LogEnrichers.Serilog;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Formatting.Json;
using VistaLOS.Application.Api.Handlers;
using VistaLOS.Application.Api.HostedServices;
using VistaLOS.Application.Api.Mapping;
using VistaLOS.Application.Api.Middlewares;
using VistaLOS.Application.Api.Options;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Data.Master.Extensions;
using VistaLOS.Application.Data.Master.Options;
using VistaLOS.Application.Data.Tenant.Extensions;
using VistaLOS.Application.Identity.Data.Extensions;
using VistaLOS.Application.Identity.Data.Options;
using VistaLOS.Application.Identity.Services.Extensions;
using VistaLOS.Application.Identity.Services.Impl;
using VistaLOS.Application.Identity.Services.Options;

namespace VistaLOS.Application.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandler>();
        services.AddTransient<TenantResolverMiddleware>();
        //services.AddTransient<RateLimitMiddleware>();

        return services;
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var masterDbOptions = configuration.GetRequiredOption<MasterDbOptions>(MasterDbOptions.Section);
        var identityDbOptions = configuration.GetRequiredOption<IdentityDbOptions>(IdentityDbOptions.Section);

        services.AddMasterDbContext(masterDbOptions);
        services.AddTenantDbContext();
        services.AddIdentityDbContext(identityDbOptions);

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddMasterRepositories();
        services.AddTenantRepositories();
        services.AddIdentityRepositories();

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection serviceCollection,
        CorsPolicyOptions corsPolicyOptions)
    {
        var allowedOrigins = corsPolicyOptions.AllowedOrigins.Split(',');
        serviceCollection.AddCors(options => {
            options.AddPolicy(corsPolicyOptions.PolicyName, builder => {
                builder
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetPreflightMaxAge(TimeSpan.FromDays(1))
                    .WithOrigins(allowedOrigins)
                    .Build();
            });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
    {
        services.AddIdentityMappingProfiles();

        services.AddAutoMapper(typeof(ApplicationMappingProfile));

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<MigrationHostedService>();

        return services;
    }

    public static void AddLogging(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Logging.ClearProviders();

        Console.Title = "Application";

        var logConfigOptions = configuration.GetRequiredOption<LoggingConfigOptions>(LoggingConfigOptions.Section);

        builder.Host.UseSerilog((_, sp, lc) => {
            lc.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            lc.Enrich.FromLogContext();
            lc.Enrich.WithSpan();

            lc.Enrich.WithThreadName();
            lc.Enrich.WithThreadId();
            lc.Enrich.WithNewRelicLogsInContext();

            lc.ReadFrom.Configuration(configuration);

            lc.Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger") ||
                    p.Value.ToString().Contains("health")));

            var hostEnv = sp.GetRequiredService<IHostEnvironment>();
            if (hostEnv.IsLocal()) {
                lc.WriteTo.Async(a => a.Console());
            }
            else {
                lc.WriteTo.Async(a => a.Console(new JsonFormatter("\r\n\r\n")));
            }
        });

        var options = configuration.GetOption<OpenTelemetryOptions>(OpenTelemetryOptions.Section);

        builder.Logging.AddOpenTelemetry(logOptions => {
            logOptions.IncludeScopes = true;
            logOptions.ParseStateValues = true;
            logOptions.IncludeFormattedMessage = true;
            if (!string.IsNullOrEmpty(options?.Endpoint)) {
                logOptions.AddOtlpExporter(string.Empty, otlpOptions => otlpOptions.Endpoint = new Uri(options.Endpoint));
            }
        });
    }

    public static IServiceCollection AddIdentityAuthLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetRequiredOption<AuthOptions>(AuthOptions.Section);

        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(
            source: Convert.FromBase64String(authOptions.PrivateKey),
            bytesRead: out var _
        );
        var securityKey = new RsaSecurityKey(rsa);
        services.AddSingleton(new SecurityKeyHolder(securityKey));

        services.AddAuthentication(options => {
            options.DefaultScheme = ApplicationAuthSchemes.TenantBearer;
        }).AddJwtBearer(ApplicationAuthSchemes.TenantBearer, options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }).AddScheme<AuthenticationSchemeOptions, AdminAuthenticationHandler>(
                ApplicationAuthSchemes.AdminFlow, options => { });

        return services;
    }

    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredOption<OpenTelemetryOptions>(OpenTelemetryOptions.Section);

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        services.AddOpenTelemetry()
            .WithTracing((builder) => {
                builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(options.ServiceName))
                    .AddHangfireInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    //.AddRedisInstrumentation()
                    .AddHttpClientInstrumentation();

                if (!string.IsNullOrEmpty(options?.Endpoint)) {
                    builder.AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = new Uri(options.Endpoint));
                }
            });

        return services;
    }

    public static IServiceCollection AddSwaggerLayer(this IServiceCollection services)
    {
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Multitenancy", Version = "v1" });
            c.AddSecurityDefinition("Api Key Auth", new OpenApiSecurityScheme {
                Description = "ApiKey must appear in header",
                Type = SecuritySchemeType.ApiKey,
                Name = ApplicationHeaders.AdminFlowKey,
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            c.AddSecurityDefinition("Bearer Auth", new OpenApiSecurityScheme() {
                Description = $"JWT Authorization header using the {ApplicationAuthSchemes.TenantBearer} scheme.",
                Type = SecuritySchemeType.ApiKey,
                Name = "Authorization",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = ApplicationAuthSchemes.TenantBearer
            });

            var requirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Api Key Auth" }
                    },
                    new[] { "DemoSwaggerDifferentAuthScheme" }
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer Auth" }
                    },
                    new[] { "DemoSwaggerDifferentAuthScheme" }
                }
            };

            c.AddSecurityRequirement(requirement);
        });

        return services;
    }
}

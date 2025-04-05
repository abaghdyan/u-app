using VistaLOS.Application.Common.Constants;

namespace VistaLOS.Application.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddFleetConfigurationSources(this WebApplicationBuilder builder)
    {
        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile("appsettings.local.json", optional: true)
                     .AddEnvironmentVariables();

        return builder;
    }

    public static IApplicationBuilder EnsureEnvironmentNameIsCorrect(this WebApplication app)
    {
        if (!VistaEnvironments.AvailableEnvironments.Contains(app.Environment.EnvironmentName)) {
            throw new ApplicationException($"Wrong application environment '{app.Environment.EnvironmentName}'.");
        }

        return app;
    }
}

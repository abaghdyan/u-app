namespace VistaLOS.Application.Api.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ValidateScopesOnBuild(this IHostBuilder builder)
    {
        return builder.UseDefaultServiceProvider(delegate (HostBuilderContext _, ServiceProviderOptions options) {
            options.ValidateOnBuild = true;
            options.ValidateScopes = true;
        });
    }
}

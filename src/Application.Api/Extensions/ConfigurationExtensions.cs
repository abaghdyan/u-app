namespace VistaLOS.Application.Api.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredOption<T>(this IConfiguration configuration, string section)
    {
        var options = configuration.GetSection(section).Get<T>();

        if (options == null) {
            throw new ArgumentNullException($"{typeof(T).Name} option was null");
        }

        return options;
    }

    public static T? GetOption<T>(this IConfiguration configuration, string section)
    {
        var options = configuration.GetSection(section).Get<T>();

        return options;
    }
}


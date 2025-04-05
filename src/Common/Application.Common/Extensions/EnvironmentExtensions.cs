using Microsoft.Extensions.Hosting;
using VistaLOS.Application.Common.Constants;

namespace VistaLOS.Application.Common.Extensions;

public static class EnvironmentExtensions
{
    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null) {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.IsEnvironment(VistaEnvironments.Local);
    }
}

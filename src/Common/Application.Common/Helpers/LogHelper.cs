using Serilog.Context;

namespace VistaLOS.Application.Common.Helpers;

public static class LogHelper
{
    private const string ModulePropertyName = "Module";

    public static IDisposable SetModuleScope(string? module) =>
        SetNamedProperty(ModulePropertyName, module);
    
    public static IDisposable SetNamedProperty(string name, string? value)
    {
        if (value != null) {
            return LogContext.PushProperty(name, value);
        }

        return Enumerable.Empty<object>().GetEnumerator();
    }
}

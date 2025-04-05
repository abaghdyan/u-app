using System.Reflection;

namespace VistaLOS.Application.Common.Constants;

public static class VistaEnvironments
{
    public const string Local = nameof(Local);
    public const string Development = nameof(Development);
    public const string QA = nameof(QA);
    public const string Production = nameof(Production);

    public static readonly IReadOnlyList<string> AvailableEnvironments;

    static VistaEnvironments()
    {
        AvailableEnvironments = typeof(VistaEnvironments)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .Select(p => p.GetValue(null)!.ToString()!)
            .ToList();
    }
}

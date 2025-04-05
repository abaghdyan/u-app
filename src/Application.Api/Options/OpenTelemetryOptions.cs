namespace VistaLOS.Application.Api.Options;

public class OpenTelemetryOptions
{
    public const string Section = "OpenTelemetry";

    public string ServiceName { get; set; } = null!;
    public string? Endpoint { get; set; }
}

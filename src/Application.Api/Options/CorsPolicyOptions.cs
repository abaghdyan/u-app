namespace VistaLOS.Application.Api.Options;

public class CorsPolicyOptions
{
    public const string Section = "CorsPolicy";

    public string PolicyName { get; set; } = null!;
    public string AllowedOrigins { get; set; } = null!;
}

namespace VistaLOS.Application.Services.Models.RateLimiting;

public class LimitationPlan
{
    public RequestLimitModel RequestLimit { get; set; } = null!;
    public RateLimitModel RateLimit { get; set; } = null!;
}

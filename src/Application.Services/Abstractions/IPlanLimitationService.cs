using VistaLOS.Application.Services.Models.RateLimiting;

namespace VistaLOS.Application.Services.Abstractions;

public interface IPlanLimitationService
{
    Task<bool> IsRateLimitExceededAsync(string limitationScope, string tenantId, RateLimitModel plan);
    Task<bool> IsRequestLimitExceededAsync(string tenantId, RequestLimitModel plan);
    Task IncrementTenantRequestCountAsync(string tenantId);
}

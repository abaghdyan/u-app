using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Constants;
using VistaLOS.Application.Services.Models.RateLimiting;
using static VistaLOS.Application.Common.Constants.CacheConstants;

namespace VistaLOS.Application.Services.Impl;

public class PlanLimitationService : IPlanLimitationService
{
    private readonly CacheConnector _cacheConnector;

    public PlanLimitationService(CacheConnector cacheConnector)
    {
        _cacheConnector = cacheConnector;
    }

    public async Task<bool> IsRateLimitExceededAsync(string limitationScope, string tenantId, RateLimitModel rateLimit)
    {
        var tenantRateLimitKey = PlanLimitation.TenantRateLimitationById.GetKey(tenantId, limitationScope);
        var limited = (int)await _cacheConnector.RedisDatabase.ScriptEvaluateAsync(RedisScripts.SlidingRateLimiterScript,
            new { key = tenantRateLimitKey, window = rateLimit.TimeWindowInSec, max_requests = rateLimit.RequestCount }) == 1;

        return limited;
    }

    public async Task<bool> IsRequestLimitExceededAsync(string tenantId, RequestLimitModel requestLimit)
    {
        var tenantRequestLimitKey = PlanLimitation.TenantRequestLimitationById.GetKey(tenantId);
        var tenantRequestMaxUsageKey = PlanLimitation.TenantRequestMaxUsageById.GetKey(tenantId);
        var limited = (int)await _cacheConnector.RedisDatabase.ScriptEvaluateAsync(RedisScripts.SlidingRequestLimiterScript,
            new {
                key = tenantRequestLimitKey,
                maxUsageKey = tenantRequestMaxUsageKey,
                maxCount = requestLimit.Quantity,
                allowScale = requestLimit.AllowScale
            }) == 1;

        return limited;
    }

    public async Task IncrementTenantRequestCountAsync(string tenantId)
    {
        var key = PlanLimitation.TenantRequestLimitationById.GetKey(tenantId);
        await _cacheConnector.RedisDatabase.ScriptEvaluateAsync(RedisScripts.SlidingIncrementScript,
                                                                new { key });
    }
}

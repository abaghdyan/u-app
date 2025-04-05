using Application.ExternalContracts.ResponseModels;
using Microsoft.AspNetCore.Authentication;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Common.Helpers;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models.RateLimiting;

namespace VistaLOS.Application.Api.Middlewares;

public class RateLimitMiddleware : IMiddleware
{
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IPlanLimitationService _planLimitationService;

    public RateLimitMiddleware(IUserContextAccessor userContextAccessor,
        IPlanLimitationService planLimitationService)
    {
        _userContextAccessor = userContextAccessor;
        _planLimitationService = planLimitationService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authenticateResultFeature = context.Features.Get<IAuthenticateResultFeature>();
        var authenticationTicket = authenticateResultFeature?.AuthenticateResult?.Ticket;

        if (authenticationTicket?.AuthenticationScheme != ApplicationAuthSchemes.TenantBearer) {
            await next(context);
            return;
        }

        var userContext = _userContextAccessor.GetRequiredUserContext();
        if (await IsRateLimitExceededAsync(context.Request.Path, userContext.TenantId)) {
            var statusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var baseResponse = new BaseResponse(statusCode, "Rate limit exceeded!");
            await context.Response.WriteAsync(JsonHelper.SerializeObject(baseResponse));
            return;
        }

        await next(context);
        await _planLimitationService.IncrementTenantRequestCountAsync(userContext.TenantId);
    }

    private async Task<bool> IsRateLimitExceededAsync(string endPointName, string tenantId)
    {
        var plan = new LimitationPlan {
            RateLimit = new RateLimitModel { RequestCount = 100, TimeWindowInSec = 10 },
            RequestLimit = new RequestLimitModel { Quantity = 10000 }
        };

        var isLimitExceeded = await _planLimitationService.IsRateLimitExceededAsync(endPointName, tenantId, plan.RateLimit);

        return isLimitExceeded;
    }
}

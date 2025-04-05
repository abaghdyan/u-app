using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Exceptions;
using VistaLOS.Application.Common.Helpers;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Data.Master.Helpers;
using VistaLOS.Application.Data.Master.Options;

namespace VistaLOS.Application.Api.Middlewares;

public class TenantResolverMiddleware : IMiddleware
{
    private readonly IUserContextWriter _userContextWriter;
    private readonly MasterDbContext _masterDbContext;
    private readonly MasterDbOptions _options;

    public TenantResolverMiddleware(IUserContextWriter userContextWriter,
        MasterDbContext masterDbContext,
        MasterDbOptions options)
    {
        _options = options;
        _userContextWriter = userContextWriter;
        _masterDbContext = masterDbContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authenticateResultFeature = context.Features.Get<IAuthenticateResultFeature>();
        var authenticationTicket = authenticateResultFeature?.AuthenticateResult?.Ticket;

        if (authenticationTicket?.AuthenticationScheme != ApplicationAuthSchemes.TenantBearer) {
            await next(context);
            return;
        }

        var authHeader = context.Request?.Headers.Authorization.ToString();
        var token = authHeader?.Replace("Bearer ", string.Empty).Replace("bearer ", string.Empty);

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var userIdString = GetRequiredClaim(jwtSecurityToken, ApplicationClaims.UserId);
        var tenantId = GetRequiredClaim(jwtSecurityToken,  ApplicationClaims.TenantId);
        var serializedPermissionIds = GetRequiredClaim(jwtSecurityToken, ApplicationClaims.PermissionIds);

        var permissionIds = JsonHelper.DeserializeObject<List<long>>(serializedPermissionIds);
        if (permissionIds == null) {
            throw new CoreException($"Incorrect {ApplicationClaims.PermissionIds} claim format.");
        }

        if (!long.TryParse(userIdString, out long userId)) {
            throw new CoreException($"Incorrect {ApplicationClaims.UserId} claim format.");
        }

        var tenant = await _masterDbContext.Tenants
            .Include(t => t.TenantStorage)
            .Where(t => t.Id == tenantId)
            .FirstOrDefaultAsync();

        if (tenant == null) {
            throw new ArgumentNullException($"Tenant with {tenantId} Id was not found.");
        }

        var connectionBuilder = ConnectionHelper.GetConnectionBuilder(_options.EncryptionKey, tenant.TenantStorage);
        var userContext = UserContext.Create(tenant.Id, userId, permissionIds, connectionBuilder.ToString());
        _userContextWriter.SetUserContext(userContext);

        await next(context);
    }

    private static string GetRequiredClaim(JwtSecurityToken jwtSecurityToken, string name)
    {
        var requiredClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == name);
        if (requiredClaim == null) {
             throw new CoreException($"{name} claim not found");
        }

        return requiredClaim.Value;
    }
}

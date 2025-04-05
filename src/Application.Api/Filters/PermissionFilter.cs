using Application.ExternalContracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Identity.Services.Managers;

namespace VistaLOS.Application.Api.Filters;

public class PermissionFilter : IAsyncAuthorizationFilter
{
    private readonly Permissions _requiredPermission;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IdentityUserManager _identityUserManager;

    public PermissionFilter(Permissions requiredPermission,
        IUserContextAccessor userContextAccessor,
        IdentityUserManager identityUserManager)
    {
        _requiredPermission = requiredPermission;
        _userContextAccessor = userContextAccessor;
        _identityUserManager = identityUserManager;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userContext = _userContextAccessor.GetRequiredUserContext();

        var user = await _identityUserManager.GetUserByIdAsync(userContext.UserId);

        if (user == null || user.Status != UserStatus.Active) {
            context.Result = ReturnUnauthorizedResult();
            return;
        }

        if (user.ChangePasswordRequired) {
            context.Result = ReturnForbiddenResult();
            return;
        }

        var permissionExist = userContext.PermissionIds.Any(pId => pId == (long)_requiredPermission);

        if (!permissionExist) {
            context.Result = ReturnForbiddenResult();
            return;
        }
    }

    private static ObjectResult ReturnUnauthorizedResult()
    {
        var response = new BaseResponse(StatusCodes.Status401Unauthorized, "Please sign in");

        var result = new ObjectResult(response) { StatusCode = response.StatusCode };

        return result;
    }

    private static ObjectResult ReturnForbiddenResult()
    {
        var response = new BaseResponse(StatusCodes.Status403Forbidden, "You don't have permission");

        var result = new ObjectResult(response) { StatusCode = response.StatusCode };

        return result;
    }
}
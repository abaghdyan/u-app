using Microsoft.AspNetCore.Authorization;
using VistaLOS.Application.Common.Constants;
using VistaLOS.Application.Common.Multitenancy;

namespace VistaLOS.Application.Api.Controllers.Base;

[Authorize(AuthenticationSchemes = ApplicationAuthSchemes.TenantBearer)]
public class TenantBaseController : AppBaseController
{
    public IUserContextAccessor IUserContextAccessor { get; }

    public TenantBaseController(IUserContextAccessor userContextAccessor)
    {
        IUserContextAccessor = userContextAccessor;
    }
}

using Microsoft.AspNetCore.Authorization;
using VistaLOS.Application.Common.Constants;

namespace VistaLOS.Application.Api.Controllers.Base;

[Authorize(AuthenticationSchemes = ApplicationAuthSchemes.AdminFlow)]
public class MasterBaseController : AppBaseController
{
}

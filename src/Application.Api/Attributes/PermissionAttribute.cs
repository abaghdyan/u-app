using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Filters;
using VistaLOS.Application.Common.Identity;

namespace VistaLOS.Application.Api.Attributes;

public class PermissionAttribute : TypeFilterAttribute
{
    public PermissionAttribute(Permissions requiredPermission)
        : base(typeof(PermissionFilter))
    {
        Arguments = [requiredPermission];
    }
}
using Application.ExternalContracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using VistaLOS.Application.Api.Attributes;
using VistaLOS.Application.Api.Controllers.Base;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models.UserManagement.Role;

namespace VistaLOS.Application.Api.Controllers.RoleManagement;

[ApiController]
[Route("api/v1/[controller]")]
public class RolesController : TenantBaseController
{
    private readonly IRoleService _roleService;

    public RolesController(IUserContextAccessor userContextAccessor,
        IRoleService roleService)
        : base(userContextAccessor)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [Permission(Permissions.ViewRole)]
    public async Task<ActionResult<DataResponse<List<RoleDto>>>> GetRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return ApiResponse(roles);
    }

    [HttpGet("{roleId}")]
    [Permission(Permissions.ViewRole)]
    public async Task<ActionResult<DataResponse<RoleDto?>>> GetRoleById(long roleId)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId);
        return ApiResponse(role);
    }
}
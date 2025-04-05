using AutoMapper;
using VistaLOS.Application.Identity.Services.Managers;
using VistaLOS.Application.Services.Abstractions;
using VistaLOS.Application.Services.Models.UserManagement.Role;

namespace VistaLOS.Application.Services.Impl;

public class RoleService : IRoleService
{
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IMapper _mapper;

    public RoleService(IdentityRoleManager identityRoleManager,
        IMapper mapper)
    {
        _identityRoleManager = identityRoleManager;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var roles = await _identityRoleManager.GetRolesAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetRoleByIdAsync(long roleId)
    {
        var role = await _identityRoleManager.GetRoleByIdAsync(roleId);

        if (role == null) {
            return null;
        }

        return _mapper.Map<RoleDto>(role);
    }
}

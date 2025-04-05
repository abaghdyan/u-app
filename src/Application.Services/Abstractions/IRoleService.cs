using VistaLOS.Application.Services.Models.UserManagement.Role;

namespace VistaLOS.Application.Services.Abstractions;

public interface IRoleService
{
    Task<List<RoleDto>> GetRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(long roleId);
}

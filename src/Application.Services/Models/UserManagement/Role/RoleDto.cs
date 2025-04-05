using VistaLOS.Application.Identity.Services.Models.Permissions;

namespace VistaLOS.Application.Services.Models.UserManagement.Role;

public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<PermissionModel> Permissions { get; set; } = new();
}

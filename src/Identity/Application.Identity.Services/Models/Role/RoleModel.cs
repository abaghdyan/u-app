using VistaLOS.Application.Identity.Services.Models.Permissions;

namespace VistaLOS.Application.Identity.Services.Models.Role;

public class RoleModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<PermissionModel> Permissions { get; set; } = new();
}

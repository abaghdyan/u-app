namespace VistaLOS.Application.Identity.Data.Entities;

public class RolePermissionEntity
{
    public long PermissionId { get; set; }
    public PermissionEntity Permission { get; set; } = null!;
    public long RoleId { get; set; }
    public RoleEntity Role { get; set; } = null!;
}

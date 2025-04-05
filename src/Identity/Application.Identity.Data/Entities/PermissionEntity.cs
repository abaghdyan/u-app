using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Data.Common.Entities;

namespace VistaLOS.Application.Identity.Data.Entities;

public class PermissionEntity : AbstractEntity, IIdentifiable<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<RolePermissionEntity> RolePermissions { get; set; } = new HashSet<RolePermissionEntity>();
}
using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Data.Common.Entities;

namespace VistaLOS.Application.Data.Master.Entities;

public class TenantGroupEntity : AbstractEntity, IIdentifiable<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }

    public ICollection<TenantEntity> Tenants { get; set; } = new HashSet<TenantEntity>();
}

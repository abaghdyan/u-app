using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Data.Common.Entities;

namespace VistaLOS.Application.Data.Master.Entities;

public class TenantStorageEntity : AbstractEntity, IIdentifiable<long>
{
    public long Id { get; set; }
    public TenantStorageStatus Status { get; set; }
    public TenantStorageType Type { get; set; }
    public string StorageName { get; set; } = null!;
    public string Server { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Location { get; set; } = null!;
    public string? ConnectionParameters { get; set; }

    public ICollection<TenantEntity> Tenants { get; set; } = new HashSet<TenantEntity>();
}

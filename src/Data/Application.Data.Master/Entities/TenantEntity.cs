using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Data.Common.Entities;

namespace VistaLOS.Application.Data.Master.Entities;

public class TenantEntity : AbstractEntity, IIdentifiable<string>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public TenantStatus Status { get; set; }
    public TenantType Type { get; set; }
    public string CompanyName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? WebSite { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? CountryIso { get; set; }
    public long TenantStorageId { get; set; }
    public long TenantGroupId { get; set; }
    public TenantStorageEntity TenantStorage { get; set; } = null!;
    public TenantGroupEntity TenantGroup { get; set; } = null!;
}

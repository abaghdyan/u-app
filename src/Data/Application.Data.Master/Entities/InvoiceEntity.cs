using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Common.Entities;

namespace VistaLOS.Application.Data.Master.Entities;

public class InvoiceEntity : AbstractEntity, IIdentifiable<long>, IHasTenantId
{
    public long Id { get; set; }
    public string TenantId { get; set; } = null!;
    public TenantEntity Tenant { get; set; } = null!;
    public DateTime Date { get; set; }
    public int Amount { get; set; }
}

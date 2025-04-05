using VistaLOS.Application.Common.Interfaces;
using VistaLOS.Application.Data.Common.Entities;

namespace VistaLOS.Application.Data.Tenant.Entities;

public class BookEntity : AbstractEntity, IIdentifiable<long>
{
    public long Id { get; set; }
    public string TenantId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int PageCount { get; set; }
}

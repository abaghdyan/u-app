using VistaLOS.Application.Data.Common.Repositories;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Repositories;

internal class InvoiceRepository : Repository<InvoiceEntity>, IInvoiceRepository
{
    public InvoiceRepository(MasterDbContext dbContext) : base(dbContext)
    {

    }
}

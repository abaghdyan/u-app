using VistaLOS.Application.Data.Common.Repositories;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Repositories;

internal class TenantRepository : Repository<TenantEntity>, ITenantRepository
{
    public TenantRepository(MasterDbContext dbContext) : base(dbContext)
    {

    }
}

using VistaLOS.Application.Data.Common.Repositories;
using VistaLOS.Application.Data.Master;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Repositories;

internal class TenantStorageRepository : Repository<TenantStorageEntity>, ITenantStorageRepository
{
    public TenantStorageRepository(MasterDbContext dbContext) : base(dbContext)
    {

    }
}

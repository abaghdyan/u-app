using Application.Identity.Data;
using VistaLOS.Application.Data.Common.Repositories;
using VistaLOS.Application.Identity.Data.Entities;

namespace VistaLOS.Application.Identity.Data.Repositories;

internal class RoleRepository : Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(IdentityDbContext dbContext) : base(dbContext)
    {

    }
}

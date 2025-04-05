using Application.Identity.Data;
using VistaLOS.Application.Data.Common.Repositories;
using VistaLOS.Application.Identity.Data.Entities;

namespace VistaLOS.Application.Identity.Data.Repositories;

internal class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(IdentityDbContext dbContext) : base(dbContext)
    {

    }
}

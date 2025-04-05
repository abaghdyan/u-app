using VistaLOS.Application.Data.Common.Repositories;
using VistaLOS.Application.Data.Tenant;
using VistaLOS.Application.Data.Tenant.Entities;

namespace VistaLOS.Application.Data.Tenant.Repositories;

internal class BookRepository : Repository<BookEntity>, IBookRepository
{
    public BookRepository(TenantDbContext dbContext) : base(dbContext)
    {

    }
}

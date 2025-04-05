using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace VistaLOS.Application.Data.Common.Repositories;

public interface IReadOnlyRepository<TEntity>
        where TEntity : class
{
    IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>?>? include = null,
            bool enableTracking = false);

    ValueTask<TEntity?> FindAsync<TKey>(TKey id);
}

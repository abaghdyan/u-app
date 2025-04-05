using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace VistaLOS.Application.Data.Common.Repositories;

public abstract class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
    where TEntity : class
{
    protected readonly DbContext _context;

    public ReadOnlyRepository(DbContext context)
    {
        _context = context;
    }

    public virtual ValueTask<TEntity?> FindAsync<TKey>(TKey id)
        => _context.Set<TEntity>().FindAsync(id);

    public virtual IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>?>? include = null,
        bool enableTracking = false)
     => GetQueryable(predicate, include, enableTracking);

    public virtual IQueryable<TEntity> BuildQuery(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>?>? include = null,
        bool enableTracking = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        var includeQuery = include?.Invoke(query);
        if (includeQuery != null) {
            query = includeQuery;
        }

        if (!enableTracking) {
            query = query.AsNoTracking();
        }

        return query;
    }

    protected IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>?>? include = null,
        bool enableTracking = false)
    {
        var query = BuildQuery(include, enableTracking);

        // Apply filters
        if (predicate is not null) {
            query = query.Where(predicate);
        }

        return query;
    }
}

using Microsoft.EntityFrameworkCore;

namespace VistaLOS.Application.Data.Common.Repositories;

public abstract class Repository<TEntity> : ReadOnlyRepository<TEntity>, IRepository<TEntity>
    where TEntity : class
{
    public Repository(DbContext context) : base(context)
    {
    }

    public virtual async Task<int> CreateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public virtual async Task<int> CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        var addRange = entities as TEntity[] ?? entities.ToArray();
        _context.Set<TEntity>().AddRange(addRange);
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public virtual async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        var tEntities = entities as TEntity[] ?? entities.ToArray();
        tEntities.ToList().ForEach(e => _context.Entry(e).State = EntityState.Modified);
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public virtual async Task<int> RemoveAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        var tEntities = entities as TEntity[] ?? entities.ToArray();
        _context.Set<TEntity>().RemoveRange(tEntities);
        var result = await _context.SaveChangesAsync();
        return result;
    }
}

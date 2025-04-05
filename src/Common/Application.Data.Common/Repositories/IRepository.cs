namespace VistaLOS.Application.Data.Common.Repositories;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class
{
    Task<int> CreateAsync(TEntity entity);
    Task<int> CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<int> UpdateAsync(TEntity entity);
    Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<int> RemoveAsync(TEntity entity);
    Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities);
}

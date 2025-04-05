using Microsoft.EntityFrameworkCore;

namespace VistaLOS.Application.Data.Common.Utilities;

public class TransactionManager<TContext> : ITransactionManager<TContext>
    where TContext : DbContext
{
    private readonly TContext _dbContext;

    public TransactionManager(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> asyncAction, CancellationToken token = default)
    {
        if (_dbContext.Database.CurrentTransaction != null) {
            return await asyncAction();
        }
        else {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () => {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
                try {
                    var result = await asyncAction();
                    await transaction.CommitAsync(token);
                    return result;
                }
                catch {
                    await transaction.RollbackAsync(token);
                    throw;
                }
            });
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Task> asyncAction, CancellationToken token = default)
    {
        if (_dbContext.Database.CurrentTransaction != null) {
            await asyncAction();
        }
        else {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
                try {
                    await asyncAction();
                    await transaction.CommitAsync(token);
                }
                catch {
                    await transaction.RollbackAsync(token);
                    throw;
                }
            });
        }
    }
}
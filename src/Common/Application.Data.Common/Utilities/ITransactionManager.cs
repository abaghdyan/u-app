using Microsoft.EntityFrameworkCore;

namespace VistaLOS.Application.Data.Common.Utilities;

public interface ITransactionManager<TContext>
    where TContext : DbContext
{
    Task ExecuteInTransactionAsync(Func<Task> asyncAction, CancellationToken token = default);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> asyncAction, CancellationToken token = default);
}
namespace VistaLOS.Application.Jobs.Abstractions;

public interface IJobService
{
    Task<string> EnqueueJobAsync<TJob, TParameter>(TParameter parameter, CancellationToken cancelationToken = default)
        where TParameter : IParameter
        where TJob : IAsyncJob<TParameter>;

    public Task<string> EnqueueRecurringJobAsync<TJob>()
        where TJob : RecurringJobBase;
}

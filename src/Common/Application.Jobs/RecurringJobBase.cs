using Hangfire;
using Hangfire.Server;
using VistaLOS.Application.Jobs.Filters;

namespace VistaLOS.Application.Jobs;

public abstract class RecurringJobBase
{
    [AutomaticRetry(Attempts = 0)]
    [JobExpirationTime]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public abstract Task ExecuteAsync(PerformContext? context);

    protected virtual string? CronExpression { get; }

    public void AddOrUpdate(string cronExpression)
    {
        RecurringJob.AddOrUpdate(() => ExecuteAsync(null), CronExpression ?? cronExpression);
    }
}

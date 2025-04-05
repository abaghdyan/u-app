using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace VistaLOS.Application.Jobs.Filters;

public class JobExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
{
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        context.JobExpirationTimeout = TimeSpan.FromDays(60);
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        context.JobExpirationTimeout = TimeSpan.FromDays(60);
    }
}

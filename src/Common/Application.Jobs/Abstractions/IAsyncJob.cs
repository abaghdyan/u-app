using Hangfire.Server;

namespace VistaLOS.Application.Jobs.Abstractions;

public interface IAsyncJob<in TParameter>
    where TParameter : IParameter
{
    Task ExecuteAsync(TParameter parameter, PerformContext context, CancellationToken cancelationToken = default);
}

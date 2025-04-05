using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace VistaLOS.Application.Api.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly ILogger _logger;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisHealthCheck(ILogger logger,
                            IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger.ForContext<RedisHealthCheck>();
        _connectionMultiplexer = connectionMultiplexer;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try {
            if (_connectionMultiplexer.IsConnected) {
                return GetResult(HealthCheckResult.Healthy());
            }
            else {
                _logger.Error("Redis HealthCheck error!");
                return GetResult(new HealthCheckResult(context.Registration.FailureStatus, "Redis is not connected"));
            }
        }
        catch (Exception ex) {
            _logger.Error("Redis HealthCheck error!");
            return GetResult(new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
        }
    }

    private Task<HealthCheckResult> GetResult(HealthCheckResult healthCheckResult)
        => Task.FromResult(healthCheckResult);
}

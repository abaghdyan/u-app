using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using VistaLOS.Application.Data.Master.Options;

namespace VistaLOS.Application.Api.HealthChecks;

public class SqlServerHealthCheck : IHealthCheck
{
    private readonly MasterDbOptions _options;
    private readonly ILogger _logger;

    public SqlServerHealthCheck(MasterDbOptions options,
                                ILogger logger)
    {
        _logger = logger.ForContext<SqlServerHealthCheck>();
        _options = options;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try {
            // Execute health check logic here.
            using (var connection = new SqlConnection(_options.ConnectionString)) {
                await connection.OpenAsync(cancellationToken);

                using (var command = connection.CreateCommand()) {
                    command.CommandText = "SELECT 1";
                    await command.ExecuteScalarAsync();
                }

                return HealthCheckResult.Healthy();
            }
        }
        catch (Exception ex) {
            _logger.Error(ex, "Sql HealthCheck error!");
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}

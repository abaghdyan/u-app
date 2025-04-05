using VistaLOS.Application.Api.HealthChecks;

namespace VistaLOS.Application.Api.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddSqlHealthCheck(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<SqlServerHealthCheck>("MasterDb");

        return builder;
    }

    public static IHealthChecksBuilder AddRedisHealthCheck(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<RedisHealthCheck>("Redis");

        return builder;
    }
}


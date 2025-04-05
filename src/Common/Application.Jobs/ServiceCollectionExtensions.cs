using Hangfire;
using Hangfire.Common;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VistaLOS.Application.Common.Extensions;
using VistaLOS.Application.Jobs;
using VistaLOS.Application.Jobs.Helpers;
using VistaLOS.Application.Jobs.Options;

namespace VistaLOS.Application.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, Func<IServiceProvider, IEnumerable<JobFilterAttribute>> filtersFunc)
    {
        services.AddHangfire((p, c) => {
            using var scope = p.CreateScope();

            var jobOptions = scope.ServiceProvider.GetService<HangfireOptions>();

            if (jobOptions == null) {
                throw new ArgumentNullException("Job options were not found");
            }

            foreach (var filter in filtersFunc.Invoke(scope.ServiceProvider)) {
                GlobalJobFilters.Filters.Add(filter);
            }

            var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            if (environment.IsLocal()) {
                DatabaseHelper.EnsureDatabaseCreated(jobOptions.ConnectionString);
            }

            c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSerilogLogProvider()
             .UseSqlServerStorage(jobOptions.ConnectionString, new SqlServerStorageOptions());
        });

        services.AddHangfireServer();

        return services;
    }

    public static void SetupRecurringJobs(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<HangfireOptions>();

        if (options.EnableRecurringJobs) {
            var jobs = app.ApplicationServices.GetServices<RecurringJobBase>().ToList();

            foreach (var job in jobs) {
                job.AddOrUpdate(options.CronExpression);
            }
        }
    }
}

namespace VistaLOS.Application.Jobs.Options;

public class HangfireOptions
{
    public const string Section = "Hangfire";

    public bool EnableRecurringJobs { get; set; }

    public string ConnectionString { get; set; } = null!;

    public string? SchedulePollingIntervalInMinutes { get; set; }

    public int? WorkerCount { get; set; }

    public string CronExpression { get; set; } = "* * * * *";
}

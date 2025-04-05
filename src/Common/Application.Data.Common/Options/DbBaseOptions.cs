namespace VistaLOS.Application.Data.Common.Options;

public abstract class DbBaseOptions
{
    public string? Schema { get; set; }
    public string ConnectionString { get; set; } = null!;
}

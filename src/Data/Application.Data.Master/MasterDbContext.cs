using Microsoft.EntityFrameworkCore;
using Serilog;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Common;
using VistaLOS.Application.Data.Master.Entities;
using VistaLOS.Application.Data.Master.Options;

namespace VistaLOS.Application.Data.Master;

public partial class MasterDbContext : CommonDbContext
{
    private const string DefaultSchemaName = "application";
    private readonly MasterDbOptions _dbOptions;

    public MasterDbContext(DbContextOptions<MasterDbContext> options,
        IUserContextAccessor userContextAccessor,
        ILogger logger,
        MasterDbOptions dbOptions)
        : base(options, userContextAccessor, logger)
    {
        _dbOptions = dbOptions;
    }

    public virtual DbSet<TenantEntity> Tenants { get; set; } = null!;
    public virtual DbSet<TenantStorageEntity> TenantStorages { get; set; } = null!;
    public virtual DbSet<PermissionEntity> Permissions { get; set; } = null!;
    public virtual DbSet<InvoiceEntity> Invoices { get; set; } = null!;

    internal static string GetSchemaName(MasterDbOptions dbOptions) => dbOptions.Schema ?? DefaultSchemaName;
}

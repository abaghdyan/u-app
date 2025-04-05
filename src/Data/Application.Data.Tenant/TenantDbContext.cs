using Microsoft.EntityFrameworkCore;
using Serilog;
using VistaLOS.Application.Common.Multitenancy;
using VistaLOS.Application.Data.Common;
using VistaLOS.Application.Data.Tenant.Entities;

namespace VistaLOS.Application.Data.Tenant;

public partial class TenantDbContext : CommonDbContext
{
    private const string DefaultSchemaName = "application";

    public TenantDbContext(DbContextOptions<TenantDbContext> options,
        IUserContextAccessor userContextAccessor,
        ILogger logger)
        : base(options, userContextAccessor, logger)
    {
    }

    public virtual DbSet<BookEntity> Books { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            var userContext = UserContextAccessor.GetUserContext();
            if (userContext != null) {
                optionsBuilder.UseSqlServer(userContext.ConnectionString);
            }
            else {
                throw new InvalidOperationException("Tenant related database connection string was not initialized");
            }
        }
    }

    internal static string GetSchemaName() => DefaultSchemaName;
}

using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Identity.Data.Entities;
using VistaLOS.Application.Identity.Data.Options;

namespace Application.Identity.Data;

public partial class IdentityDbContext : DbContext
{
    private const string DefaultSchemaName = "identity";
    internal const string MigrationTableName = "__EFMigrationsHistory";
    
    private readonly IdentityDbOptions _dbOptions;

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options,
        IdentityDbOptions dbOptions) 
        : base(options)
    {
        _dbOptions = dbOptions;
    }

    public virtual DbSet<UserEntity> Users { get; set; } = null!;
    public virtual DbSet<RoleEntity> Roles { get; set; } = null!;
    public virtual DbSet<PermissionEntity> Permissions { get; set; } = null!;
    public virtual DbSet<RolePermissionEntity> RolePermissions { get; set; } = null!;

    internal static string GetSchemaName(IdentityDbOptions dbOptions) => dbOptions.Schema ?? DefaultSchemaName;
}

using Microsoft.EntityFrameworkCore;
using VistaLOS.Application.Identity.Data.Infrastructure.Configurations;

namespace Application.Identity.Data;

public partial class IdentityDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schemaName = GetSchemaName(this._dbOptions);
        modelBuilder.HasDefaultSchema(schemaName);

        modelBuilder.ApplyConfiguration(new PermissionConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new RoleConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(schemaName));
        modelBuilder.ApplyConfiguration(new UserConfiguration(schemaName));

        base.OnModelCreating(modelBuilder);
    }
}
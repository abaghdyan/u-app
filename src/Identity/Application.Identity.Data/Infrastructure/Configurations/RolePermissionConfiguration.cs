using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Identity.Data.Constants;
using VistaLOS.Application.Identity.Data.Entities;

namespace VistaLOS.Application.Identity.Data.Infrastructure.Configurations;

internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
{
    private readonly string _schema;

    public RolePermissionConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.ToTable("RolePermissions", _schema);

        builder.HasKey(e => new { e.PermissionId, e.RoleId });

        SeedData(builder);

        CreateRelationships(builder);
    }

    private static void SeedData(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Owner, PermissionId = (long)Permissions.ViewBook });
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Owner, PermissionId = (long)Permissions.EditBook });
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Owner, PermissionId = (long)Permissions.ViewInvoice });
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Owner, PermissionId = (long)Permissions.EditInvoice });

        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Admin, PermissionId = (long)Permissions.ViewBook });
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Admin, PermissionId = (long)Permissions.EditBook });
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Admin, PermissionId = (long)Permissions.ViewInvoice });
        builder.HasData(new RolePermissionEntity { RoleId = (long)DefaultRoles.Admin, PermissionId = (long)Permissions.EditInvoice });
    }

    private static void CreateRelationships(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.HasOne(bc => bc.Permission)
            .WithMany(b => b.RolePermissions)
            .HasForeignKey(bc => bc.PermissionId);

        builder.HasOne(bc => bc.Role)
            .WithMany(b => b.RolePermissions)
            .HasForeignKey(bc => bc.RoleId);
    }
}

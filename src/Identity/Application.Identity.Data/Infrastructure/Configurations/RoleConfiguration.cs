using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Identity.Data.Constants;
using VistaLOS.Application.Identity.Data.Entities;

namespace VistaLOS.Application.Identity.Data.Infrastructure.Configurations;

internal class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    private readonly string _schema;

    public RoleConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles", _schema);

        builder.HasKey(u => u.Id);

        SeedData(builder);

        CreateRelationships(builder);
    }

    private static void SeedData(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasData(new RoleEntity { Id = (long)DefaultRoles.Owner, Name = DefaultRoles.Owner.ToString() });
        builder.HasData(new RoleEntity { Id = (long)DefaultRoles.Admin, Name = DefaultRoles.Admin.ToString() });
    }

    private static void CreateRelationships(EntityTypeBuilder<RoleEntity> builder)
    {
    }
}

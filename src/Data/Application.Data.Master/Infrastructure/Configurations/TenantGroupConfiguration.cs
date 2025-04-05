using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Data.Master.Constants;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Infrastructure.Configurations;

internal class TenantGroupConfiguration : IEntityTypeConfiguration<TenantGroupEntity>
{
    private readonly string _schema;

    public TenantGroupConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<TenantGroupEntity> builder)
    {
        builder.ToTable("TenantGroups", _schema);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<TenantGroupEntity> builder)
    {
        builder.HasData(new TenantGroupEntity {
            Id = (long)TenantGroups.Default,
            Name = TenantGroups.Default.ToString(),
        });
    }
}

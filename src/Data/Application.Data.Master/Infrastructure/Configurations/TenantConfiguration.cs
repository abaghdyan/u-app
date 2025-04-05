using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Infrastructure.Configurations;

internal class TenantConfiguration : IEntityTypeConfiguration<TenantEntity>
{
    private readonly string _schema;

    public TenantConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<TenantEntity> builder)
    {
        builder.ToTable("Tenants", _schema);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        CreateRelationships(builder);
    }

    private static void CreateRelationships(EntityTypeBuilder<TenantEntity> builder)
    {
        builder.HasOne(d => d.TenantStorage)
            .WithMany(p => p.Tenants)
            .HasForeignKey(d => d.TenantStorageId);

        builder.HasOne(d => d.TenantGroup)
            .WithMany(p => p.Tenants)
            .HasForeignKey(d => d.TenantGroupId);
    }
}

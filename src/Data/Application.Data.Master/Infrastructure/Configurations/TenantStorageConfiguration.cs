using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Infrastructure.Configurations;

internal class TenantStorageConfiguration : IEntityTypeConfiguration<TenantStorageEntity>
{
    private readonly string _schema;

    public TenantStorageConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<TenantStorageEntity> builder)
    {
        builder.ToTable("TenantStorages", _schema);

        builder.HasKey(e => e.Id);
    }
}

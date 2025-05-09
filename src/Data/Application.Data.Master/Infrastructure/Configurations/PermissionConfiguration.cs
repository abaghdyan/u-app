using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Common.Identity;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Infrastructure.Configurations;

internal class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    private readonly string _schema;

    public PermissionConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.ToTable("Permissions", _schema);
        builder.HasKey(u => u.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasData(new PermissionEntity { Id = (long)Permissions.ViewBook, Name = Permissions.ViewBook.ToString() });
        builder.HasData(new PermissionEntity { Id = (long)Permissions.EditBook, Name = Permissions.EditBook.ToString() });
        builder.HasData(new PermissionEntity { Id = (long)Permissions.ViewInvoice, Name = Permissions.ViewInvoice.ToString() });
        builder.HasData(new PermissionEntity { Id = (long)Permissions.EditInvoice, Name = Permissions.EditInvoice.ToString() });
    }
}

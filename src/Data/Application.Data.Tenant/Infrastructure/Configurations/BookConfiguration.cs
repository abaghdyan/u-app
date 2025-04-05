using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Data.Tenant.Entities;

namespace VistaLOS.Application.Data.Tenant.Infrastructure.Configurations;

internal class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
    private readonly string _schema;

    public BookConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.ToTable("Books", _schema);

        builder.HasKey(e => e.Id);
    }
}

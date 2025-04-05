using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Data.Master.Entities;

namespace VistaLOS.Application.Data.Master.Infrastructure.Configurations;

internal class InvoiceConfiguration : IEntityTypeConfiguration<InvoiceEntity>
{
    private readonly string _schema;

    public InvoiceConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
    {
        builder.ToTable("Invoices", _schema);

        builder.HasKey(e => e.Id);
    }
}

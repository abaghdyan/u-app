using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VistaLOS.Application.Identity.Data.Entities;

namespace VistaLOS.Application.Identity.Data.Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    private readonly string _schema;

    public UserConfiguration(string schema)
    {
        _schema = schema;
    }

    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users", _schema);

        builder.HasKey(u => u.Id);

        CreateRelationships(builder);
    }

    private static void CreateRelationships(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasOne(bc => bc.Role)
            .WithMany(b => b.Users)
            .HasForeignKey(bc => bc.RoleId);
    }
}

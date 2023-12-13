using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class MeasureUnitMappings : IEntityTypeConfiguration<MeasureUnit>
{
    public void Configure(EntityTypeBuilder<MeasureUnit> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id");

        builder.Property(m => m.Name)
            .HasColumnName("Name")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(m => m.ShortName)
            .HasColumnName("ShortName")
            .HasMaxLength(40);

        builder.Property(m => m.Symbol)
            .HasColumnName("Symbol")
            .HasMaxLength(4)
            .IsRequired();
    }
}
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class Product_MeasureDetailMappings : IEntityTypeConfiguration<MeasurementDetail>
{
    public void Configure(EntityTypeBuilder<MeasurementDetail> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(m => m.Value)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(m => m.ShowOrder)
            .IsRequired();

        builder.HasOne(m => m.MeasureUnit)
            .WithMany();
    }
}
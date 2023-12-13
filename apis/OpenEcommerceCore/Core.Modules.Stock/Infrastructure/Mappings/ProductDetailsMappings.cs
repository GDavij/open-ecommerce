using Core.Modules.Stock.Domain.Entities.Complex.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class ProductDetailsMappings : IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.Id)
            .HasColumnName("Id");

        builder.Property(pd => pd.Name)
            .HasColumnName("Name")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(pd => pd.Value)
            .HasColumnName("Value")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(pd => pd.ShowOrder)
            .HasColumnName("ShowOrder")
            .IsRequired();

        builder.HasOne(pd => pd.MeasureUnit)
            .WithOne()
            .OnDelete(DeleteBehavior.);
        
        //TODO: Model complex relationship
    }
}
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

        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.Measurements)
            .HasForeignKey(pd => pd.Product.Id)
            .IsRequired();
        
        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.TechnicalDetails)
            .HasForeignKey(pd => pd.Product.Id)
            .IsRequired();
        
        builder.HasOne(pd => pd.Product)
            .WithMany(p => p.OtherDetails)
            .HasForeignKey(pd => pd.Product.Id)
            .IsRequired();
        
        builder.Property(pd => pd.ShowOrder)
            .HasColumnName("ShowOrder")
            .IsRequired();
        
        builder.Property(pd => pd.Name)
            .HasColumnName("Name")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(pd => pd.Value)
            .HasColumnName("Value")
            .HasMaxLength(128)
            .IsRequired();
        
        builder.HasOne(pd => pd.MeasureUnit)
            .WithMany(m => m.ProductDetails)
            .HasForeignKey(pd => pd.MeasureUnit.Id);
    }
}
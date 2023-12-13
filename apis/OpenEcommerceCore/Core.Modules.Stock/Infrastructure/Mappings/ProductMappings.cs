using Core.Modules.Stock.Domain.Entities.Complex.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class ProductMappings : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id");

        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.Brand.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("Description")
            .HasMaxLength(512);

        builder.HasMany(p => p.Tags)
            .WithOne()
            .HasForeignKey(t => t.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(i => i.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.SKU)
            .HasColumnName("SKU")
            .HasMaxLength(20);

        builder.Property(p => p.EAN)
            .HasColumnName("EAN_13")
            .HasMaxLength(13)
            .IsRequired();

        builder.Property(p => p.UPC)
            .HasColumnName("UPC_A")
            .HasMaxLength(12);

        builder.Property(p => p.Price)
            .HasColumnName("Price")
            .IsRequired();

        builder.Property(p => p.StockUnitCount)
            .HasColumnName("StockUnitCount")
            .IsRequired();

        builder.HasMany(p => p.Measurements)
            .WithOne(m => m.Product)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.TechnicalDetails)
            .WithOne(t => t.Product)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.OtherDetails)
            .WithOne(o => o.Product)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(p => p.LastUpdate)
            .HasColumnName("LastUpdate")
            .IsRequired();
    }
}
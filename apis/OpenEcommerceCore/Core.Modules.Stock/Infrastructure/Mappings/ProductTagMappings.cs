using Core.Modules.Stock.Domain.Entities.Complex.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class ProductTagMappings : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .HasColumnName("Id");

        builder.Property(pt => pt.Name)
            .HasColumnName("TagName")
            .HasMaxLength(128)
            .IsRequired();

        builder.HasMany(pt => pt.TaggedProducts)
            .WithMany(p => p.Tags);
    }
}
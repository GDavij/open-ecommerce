using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class BrandMappings : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("Id");
        
        builder.Property(b => b.Name)
            .HasColumnName("Name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.Description)
            .HasColumnName("Description")
            .HasMaxLength(512);

        builder.HasMany(b => b.Products)
            .WithOne(p => p.Brand);
    }
}
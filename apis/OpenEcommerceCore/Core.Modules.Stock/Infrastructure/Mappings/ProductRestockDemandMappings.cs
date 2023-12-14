using Core.Modules.Stock.Domain.Entities.Demands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class ProductRestockDemandMappings : IEntityTypeConfiguration<ProductRestockDemand>
{
    public void Configure(EntityTypeBuilder<ProductRestockDemand> builder)
    {
        builder.HasKey(prd => prd.Id);

        builder.Property(prd => prd.Id)
            .HasColumnName("Id");

        builder.HasOne(prd => prd.Product)
            .WithMany(p => p.ProductRestockDemands)
            .HasForeignKey(prd => prd.Id)
            .IsRequired();

        builder.Property(prd => prd.Description)
            .HasColumnName("Description")
            .HasMaxLength(255);

        builder.Property(prd => prd.RestockQuantity)
            .HasColumnName("RestockQuantity")
            .IsRequired();

        builder.HasMany(prd => prd.DemandMessages)
            .WithOne(dm => dm.ProductRestockDemand)
            .HasForeignKey(dm => dm.Id)
            .IsRequired();
        
        builder.Property(prd => prd.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(prd => prd.LastUpdate)
            .HasColumnName("LastUpdate")
            .IsRequired();

        builder.Property(prd => prd.Resolved)
            .HasColumnName("Resolved")
            .IsRequired();

        builder.Property(prd => prd.Canceled)
            .HasColumnName("Canceled")
            .IsRequired();

        builder.Property(prd => prd.Deleted)
            .HasColumnName("Deleted")
            .IsRequired();
    }
}
using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class SupplierMappings : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("Id");

        builder.Property(s => s.Name) 
            .HasColumnName("Name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Email)
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(15);

        builder.HasMany(s => s.Products)
            .WithMany(p => p.Suppliers);

        builder.HasOne(s => s.Address)
            .WithMany(a => a.Suppliers)
            .IsRequired();
        
        builder.Property(s => s.SalesNumber)
            .HasColumnName("SalesNumber")
            .IsRequired();

    }
}
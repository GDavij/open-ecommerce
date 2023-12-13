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

        builder.Property(s => s.Address.Neighbourhood)
            .HasColumnName("Address_Neighbourhood")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Address.State)
            .HasColumnName("Address_State")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(s => s.Address.ZipCode)
            .HasColumnName("Address_ZipCode")
            .IsRequired();

        builder.Property(s => s.Address.Street)
            .HasColumnName("Address_Street")
            .HasMaxLength(128)
            .IsRequired();
        
        builder.Property(s => s.SalesNumber)
            .HasColumnName("SalesNumber")
            .IsRequired();

    }
}
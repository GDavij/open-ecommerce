using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class AddressMappings : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("Id");

        builder.Property(a => a.Neighbourhood)
            .HasColumnName("Neighbourhood")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.State)
            .HasColumnName("State")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(a => a.ZipCode)
            .HasColumnName("ZipCode")
            .IsRequired();

        builder.Property(a => a.Street)
            .HasColumnName("Street")
            .HasMaxLength(128)
            .IsRequired();

        builder.HasMany(a => a.Suppliers)
            .WithOne(s => s.Address);
    }
}
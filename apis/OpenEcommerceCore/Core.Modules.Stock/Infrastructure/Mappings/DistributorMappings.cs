using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class DistributorMappings : IEntityTypeConfiguration<Distributor>
{
    public void Configure(EntityTypeBuilder<Distributor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("Id");

        builder.Property(d => d.Email)
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("Name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.Phone)
            .HasColumnName("Phone")
            .HasMaxLength(15);

        builder.Property(d => d.SalesNumber)
            .HasColumnName("SalesNumber")
            .IsRequired();

        builder.Property(d => d.Address.Neighbourhood)
            .HasColumnName("Address_Neighbourhood")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.Address.State)
            .HasColumnName("Address_State")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(d => d.Address.ZipCode)
            .HasColumnName("Address_ZipCode")
            .IsRequired();

        builder.Property(d => d.Address.Street)
            .HasColumnName("Address_Street")
            .HasMaxLength(128)
            .IsRequired();
    }
}
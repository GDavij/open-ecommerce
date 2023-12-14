using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class DemandMessageMappings : IEntityTypeConfiguration<DemandMessage>
{
    public void Configure(EntityTypeBuilder<DemandMessage> builder)
    {
        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.Id)
            .HasColumnName("Id");
        
        builder.HasOne(dm => dm.ProductRestockDemand)
            .WithMany(pro => pro.DemandMessages)
            .HasForeignKey(dm => dm.Id)
            .IsRequired();

        builder.HasOne(dm => dm.Collaborator)
            .WithMany(c => c.DemandMessages)
            .HasForeignKey(dm => dm.Id)
            .IsRequired();

        builder.Property(dm => dm.Sector)
            .HasColumnName("FromSector")
            .IsRequired();

        builder.Property(dm => dm.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(dm => dm.Message)
            .HasColumnName("Message")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(dm => dm.Deleted)
            .HasColumnName("Deleted")
            .IsRequired();
    }
}
using Core.Modules.Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.Stock.Infrastructure.Mappings;

internal class CollaboratorMappings : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id");

        builder.Property(c => c.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(c => c.DemandMessages)
            .WithOne(dm => dm.Collaborator)
            .HasForeignKey(dm => dm.Collaborator.Id);

        builder.Property(c => c.Deleted)
            .HasColumnName("Deleted")
            .IsRequired();
    }
}
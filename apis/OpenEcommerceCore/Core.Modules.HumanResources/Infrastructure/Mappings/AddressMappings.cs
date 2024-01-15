using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class AddressMappings : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne<Collaborator>(a => a.Collaborator)
            .WithMany(c => c.Addresses)
            .HasForeignKey(a => a.CollaboratorId)
            .IsRequired();

        builder.Property(a => a.State)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(a => a.ZipCode)
            .IsRequired();

        builder.Property(a => a.Neighbourhood)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.Street)
            .HasMaxLength(128)
            .IsRequired();
    }
}
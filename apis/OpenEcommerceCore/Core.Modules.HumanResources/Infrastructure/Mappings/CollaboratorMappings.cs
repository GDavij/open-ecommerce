using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class CollaboratorMappings : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(255);

        builder.Property(c => c.Description)
            .HasMaxLength(2048);
        
        builder.Property(c => c.Age)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Phone)
            .HasMaxLength(22)
            .IsRequired();

        builder.HasMany<Contract>(c => c.Contracts)
            .WithOne(c => c.Collaborator)
            .HasForeignKey(c => c.CollaboratorId);

        builder.HasMany<SocialLink>(c => c.SocialLinks)
            .WithOne(s => s.Collaborator)
            .HasForeignKey(s => s.CollaboratorId);

        builder.HasMany<Address>(c => c.Addresses)
            .WithOne(a => a.Collaborator)
            .HasForeignKey(a => a.CollaboratorId);

        builder.Ignore(c => c.TotalContributionYears);

        builder.Ignore(c => c.TotalHoursWorked);

    }
}
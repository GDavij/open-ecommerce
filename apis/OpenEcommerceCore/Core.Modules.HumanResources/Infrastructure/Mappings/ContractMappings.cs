using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class ContractMappings : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne<Collaborator>(c => c.Collaborator)
            .WithMany(c => c.Contracts)
            .HasForeignKey(c => c.CollaboratorId)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Sector)
            .IsRequired();
        
        builder.HasMany<ContributionYear>(c => c.ContributionYears)
            .WithOne(c => c.Contract)
            .HasForeignKey(c => c.ContractId);

        builder.Property(c => c.StartDate)
            .IsRequired();

        builder.Property(c => c.EndDate)
            .IsRequired();

        builder.Property(c => c.MonthlySalary)
            .IsRequired();
        
        builder.Ignore(c => c.Expired);

        builder.Property(c => c.Broken)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.Deleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}
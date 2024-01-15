using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class ContributionYearMappings : IEntityTypeConfiguration<ContributionYear>
{
    public void Configure(EntityTypeBuilder<ContributionYear> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Contract)
            .WithMany(c => c.ContributionYears)
            .HasForeignKey(c => c.ContractId);

        builder.Property(c => c.Year)
            .IsRequired();

        builder.HasMany<WorkHour>(c => c.WorkHours)
            .WithOne(c => c.ContributionYear)
            .HasForeignKey(c => c.ContributionYearId);
    }
}
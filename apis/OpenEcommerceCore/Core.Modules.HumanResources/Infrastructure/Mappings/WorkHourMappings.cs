using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class WorkHourMappings : IEntityTypeConfiguration<WorkHour>
{
    public void Configure(EntityTypeBuilder<WorkHour> builder)
    {
        builder.HasKey(w => w.Id);

        builder.HasOne<ContributionYear>(w => w.ContributionYear)
            .WithMany(c => c.WorkHours)
            .HasForeignKey(w => w.ContributionYearId)
            .IsRequired();

        builder.Property(w => w.Date)
            .IsRequired();

        builder.Property(w => w.Start)
            .IsRequired();

        // Need to be filled at the end of the day
        builder.Property(w => w.End);

        builder.Ignore(w => w.Duration);
    }
}
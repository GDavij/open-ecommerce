using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class StateMappings : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(s => s.ShortName)
            .HasMaxLength(4)
            .IsRequired();

        builder.HasMany(s => s.Addresses)
            .WithOne(a => a.State)
            .HasForeignKey(a => a.StateId);
    }
}
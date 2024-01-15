using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class SocialLinkMappings : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasOne<Collaborator>(s => s.Collaborator)
            .WithMany(c => c.SocialLinks)
            .HasForeignKey(s => s.CollaboratorId);

        builder.HasOne<JobApplication>(s => s.JobApplication)
            .WithMany(j => j.SocialLinks)
            .HasForeignKey(s => s.JobApplicationId);

        builder.Property(s => s.SocialMedia)
            .IsRequired();

        builder.Property(s => s.URL)
            .HasMaxLength(384)
            .IsRequired();
    }
}
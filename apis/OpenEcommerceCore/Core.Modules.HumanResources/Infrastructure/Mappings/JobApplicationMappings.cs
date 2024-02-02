using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.HumanResources.Infrastructure.Mappings;

internal class JobApplicationMappings : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(j => j.LastName)
            .HasMaxLength(255);

        builder.Property(j => j.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(j => j.Phone)
            .HasMaxLength(22)
            .IsRequired();
        
        builder.Property(j => j.Age)
            .IsRequired();

        builder.Property(j => j.Sector)
            .IsRequired();

        builder.Property(j => j.ProcessStep)
            .IsRequired();

        builder.Property(j => j.ResumeURL)
            .HasMaxLength(384)
            .IsRequired();

        builder.Property(j => j.CreatedAt)
            .IsRequired();
        
        builder.HasMany<SocialLink>(j => j.SocialLinks)
            .WithOne(s => s.JobApplication)
            .HasForeignKey(s => s.JobApplicationId);
    }
}
using Core.Modules.UserAccess.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Modules.UserAccess.Infrastructure.Mappings;

internal class CollaboratorMappings : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id");

        builder.Property(c => c.CollaboratorModuleId)
            .HasColumnName("CollaboratorModuleId")
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Password)
            .HasColumnName("Password")
            .IsRequired();

        builder.Property(c => c.SecurityKey)
            .HasColumnName("SecurityKey")
            .IsRequired();

        builder.Property(c => c.Sectors)
            .HasColumnName("Sector")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(c => c.LastLogin)
            .HasColumnName("LastLogin")
            .IsRequired();

        builder.Property(c => c.IsAdmin)
            .HasColumnName("IsAdmin")
            .IsRequired();
        
        builder.Property(c => c.Deleted)
            .HasColumnName("Deleted")
            .IsRequired();
    }
}
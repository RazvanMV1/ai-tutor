using AiTutor.Domain.Entities;
using AiTutor.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiTutor.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Email");

        builder.HasIndex(u => u.Email).IsUnique();

        // 🆕 Parent ↔ Child
        builder.Property(u => u.ParentId)
            .IsRequired(false);

        builder.Property(u => u.InvitationCode)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.HasIndex(u => u.InvitationCode).IsUnique();

        builder.HasMany(u => u.Progresses)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

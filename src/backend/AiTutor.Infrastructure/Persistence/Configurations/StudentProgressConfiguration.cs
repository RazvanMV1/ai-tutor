using AiTutor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiTutor.Infrastructure.Persistence.Configurations;

public class StudentProgressConfiguration : IEntityTypeConfiguration<StudentProgress>
{
    public void Configure(EntityTypeBuilder<StudentProgress> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => new { p.UserId, p.LessonId }).IsUnique();

        builder.Property(p => p.ScorePercentage)
            .IsRequired();

        builder.Property(p => p.AttemptsCount)
            .IsRequired();
    }
}

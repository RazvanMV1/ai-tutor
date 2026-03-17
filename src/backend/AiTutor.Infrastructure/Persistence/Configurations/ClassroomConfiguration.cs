using AiTutor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiTutor.Infrastructure.Persistence.Configurations;

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.ClassCode).IsRequired().HasMaxLength(20);
        builder.HasIndex(c => c.ClassCode).IsUnique();
        builder.Property(c => c.SubjectType).IsRequired();
        builder.Property(c => c.TeacherId).IsRequired();
        builder.Property(c => c.MaxStudents).HasDefaultValue(50);
        builder.HasMany(c => c.Members)
            .WithOne(m => m.Classroom)
            .HasForeignKey(m => m.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Lessons)
            .WithOne(l => l.Classroom)
            .HasForeignKey(l => l.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClassroomMemberConfiguration : IEntityTypeConfiguration<ClassroomMember>
{
    public void Configure(EntityTypeBuilder<ClassroomMember> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.ClassroomId).IsRequired();
        builder.Property(m => m.StudentId).IsRequired();
        builder.HasIndex(m => new { m.ClassroomId, m.StudentId }).IsUnique();
    }
}

public class ClassroomLessonConfiguration : IEntityTypeConfiguration<ClassroomLesson>
{
    public void Configure(EntityTypeBuilder<ClassroomLesson> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Title).IsRequired().HasMaxLength(200);
        builder.Property(l => l.Content).IsRequired();
        builder.HasMany(l => l.Quizzes)
            .WithOne(q => q.Lesson)
            .HasForeignKey(q => q.ClassroomLessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClassroomQuizConfiguration : IEntityTypeConfiguration<ClassroomQuiz>
{
    public void Configure(EntityTypeBuilder<ClassroomQuiz> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Title).IsRequired().HasMaxLength(200);
        builder.HasMany(q => q.Questions)
            .WithOne(qq => qq.Quiz)
            .HasForeignKey(qq => qq.ClassroomQuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClassroomQuestionConfiguration : IEntityTypeConfiguration<ClassroomQuestion>
{
    public void Configure(EntityTypeBuilder<ClassroomQuestion> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Text).IsRequired().HasMaxLength(1000);
        builder.Property(q => q.CorrectAnswer).IsRequired().HasMaxLength(500);
        builder.Property(q => q.Explanation).HasMaxLength(1000);
        builder.Property(q => q.Options)
            .HasConversion(
                v => string.Join("||", v),
                v => v.Split("||", StringSplitOptions.None).ToList())
            .Metadata.SetValueComparer(
                new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }
}

public class ClassroomProgressConfiguration : IEntityTypeConfiguration<ClassroomProgress>
{
    public void Configure(EntityTypeBuilder<ClassroomProgress> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.ClassroomId, p.StudentId, p.ClassroomLessonId }).IsUnique();
    }
}

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Value).IsRequired();
        builder.Property(g => g.Description).HasMaxLength(500);
        builder.HasIndex(g => new { g.ClassroomId, g.StudentId, g.GradedAt });
    }
}

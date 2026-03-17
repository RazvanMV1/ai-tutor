using AiTutor.Application.Common.Interfaces;
using AiTutor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.UnitTests.Application;

public class TestDbContext : DbContext, IApplicationDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<StudentProgress> StudentProgresses => Set<StudentProgress>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<ClassroomMember> ClassroomMembers => Set<ClassroomMember>();
    public DbSet<ClassroomLesson> ClassroomLessons => Set<ClassroomLesson>();
    public DbSet<ClassroomQuiz> ClassroomQuizzes => Set<ClassroomQuiz>();
    public DbSet<ClassroomQuestion> ClassroomQuestions => Set<ClassroomQuestion>();
    public DbSet<ClassroomProgress> ClassroomProgresses => Set<ClassroomProgress>();
    public DbSet<Grade> Grades => Set<Grade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().Property(u => u.Email)
            .HasConversion(e => e.Value, v => AiTutor.Domain.ValueObjects.Email.Create(v));
        modelBuilder.Entity<Question>().Property(q => q.Options)
            .HasConversion(
                o => string.Join("||", o),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList());
        modelBuilder.Entity<ClassroomQuestion>().Property(q => q.Options)
            .HasConversion(
                o => string.Join("||", o),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList());
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken)
;

}


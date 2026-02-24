using AiTutor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<Lesson> Lessons { get; }
    DbSet<Quiz> Quizzes { get; }
    DbSet<Question> Questions { get; }
    DbSet<StudentProgress> StudentProgresses { get; }
    DbSet<Subscription> Subscriptions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

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

    // Classroom
    DbSet<Classroom> Classrooms { get; }
    DbSet<ClassroomMember> ClassroomMembers { get; }
    DbSet<ClassroomLesson> ClassroomLessons { get; }
    DbSet<ClassroomQuiz> ClassroomQuizzes { get; }
    DbSet<ClassroomQuestion> ClassroomQuestions { get; }
    DbSet<ClassroomProgress> ClassroomProgresses { get; }
    DbSet<Grade> Grades { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

namespace AiTutor.Web.Models;

// Enums
public enum DifficultyLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3
}

public enum SubjectType
{
    Mathematics = 1,
    Romanian = 2,
    Informatics = 3
}

public enum SubscriptionType
{
    Free = 1,
    ParentMonthly = 2,
    ParentYearly = 3,
    School = 4
}

public enum UserRole
{
    Student = 1,
    Parent = 2,
    Teacher = 3,
    Admin = 4
}

// Auth Models
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Student;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

// Subject Models
public class SubjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SubjectType Type { get; set; }
}

public class CreateSubjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SubjectType Type { get; set; } = SubjectType.Mathematics;
}

public class CreateLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; } = 1;
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Beginner;
    public Guid SubjectId { get; set; }
}

// Lesson Models
public class LessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public Guid SubjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Quiz Models
public class QuizSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int QuestionsCount { get; set; }
}

public class QuizDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public Guid LessonId { get; set; }
    public int TimeLimitMinutes { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int Points { get; set; }
}

public class QuizAnswerDto
{
    public Guid QuestionId { get; set; }
    public string Answer { get; set; } = string.Empty;
}

public class SubmitQuizCommand
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public List<QuizAnswerDto> Answers { get; set; } = new();
}

public class QuizResultResponse
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int ScorePercentage { get; set; }
    public List<QuestionResultDto> Results { get; set; } = new();
}

public class QuestionResultDto
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string UserAnswer { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public string? Explanation { get; set; }
}

// Progress Models
public class StudentProgressDto
{
    public Guid LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int ScorePercentage { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class CompleteLessonCommand
{
    public Guid UserId { get; set; }
    public Guid LessonId { get; set; }
    public int ScorePercentage { get; set; }
}

// Subscription Models
public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public decimal Price { get; set; }
}

public class CreateSubscriptionRequest
{
    public Guid UserId { get; set; }
    public SubscriptionType Type { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

// AI Models
public class ExplanationRequest
{
    public string Topic { get; set; } = string.Empty;
    public SubjectType Subject { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int StudentAge { get; set; }
}

public class ExplanationResponse
{
    public string Topic { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public List<string> Examples { get; set; } = new();
    public List<string> KeyPoints { get; set; } = new();
    public SubjectType Subject { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
}

public class HintRequest
{
    public string Question { get; set; } = string.Empty;
    public SubjectType Subject { get; set; }
}

public class HintResponse
{
    public string Question { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public SubjectType Subject { get; set; }
}

public class ProblemsRequest
{
    public string Topic { get; set; } = string.Empty;
    public SubjectType Subject { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int StudentAge { get; set; }
    public int Count { get; set; } = 3;
}

public class ProblemResponse
{
    public string Topic { get; set; } = string.Empty;
    public List<string> Problems { get; set; } = new();
    public List<string> Hints { get; set; } = new();
    public List<string> Solutions { get; set; } = new();
    public SubjectType Subject { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
}

// Auth State
public class AuthState
{
    public bool IsAuthenticated { get; set; }
    public string? Token { get; set; }
    public UserDto? User { get; set; }
}

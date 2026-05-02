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

// Classroom Models
public class ClassroomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SubjectType SubjectType { get; set; }
    public Guid TeacherId { get; set; }
    public string ClassCode { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
    public bool IsActive { get; set; }
    public int MemberCount { get; set; }
    public int LessonCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateClassroomRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SubjectType SubjectType { get; set; } = SubjectType.Mathematics;
}

public class ClassroomMemberDto
{
    public Guid StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
}

public class ClassroomLessonDto
{
    public Guid Id { get; set; }
    public Guid ClassroomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateClassroomLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; } = 1;
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Beginner;
}

public class ClassroomQuizDto
{
    public Guid Id { get; set; }
    public Guid ClassroomLessonId { get; set; }
    public Guid ClassroomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int QuestionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateClassroomQuizRequest
{
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Beginner;
    public int TimeLimitMinutes { get; set; } = 30;
}

public class ClassroomQuestionDto
{
    public Guid Id { get; set; }
    public Guid ClassroomQuizId { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddClassroomQuestionRequest
{
    public string Text { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int Points { get; set; } = 10;
    public string? Explanation { get; set; }
}

public class UpdateClassroomQuizRequest
{
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Beginner;
    public int TimeLimitMinutes { get; set; } = 30;
}

public class ClassroomQuizDetailDto
{
    public Guid Id { get; set; }
    public Guid ClassroomLessonId { get; set; }
    public Guid ClassroomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; }
    public int TimeLimitMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ClassroomQuestionDetailDto> Questions { get; set; } = new();
}

public class ClassroomQuestionDetailDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int Points { get; set; }
    public string? Explanation { get; set; }
}


public class ClassroomProgressDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public Guid ClassroomLessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public double ScorePercentage { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class GradeDto
{
    public Guid Id { get; set; }
    public Guid ClassroomId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }
    public int Value { get; set; }
    public string? Description { get; set; }
    public DateTime GradedAt { get; set; }
}

public class AddGradeRequest
{
    public Guid StudentId { get; set; }
    public int Value { get; set; }
    public string? Description { get; set; }
}

public class UpdateGradeRequest
{
    public int Value { get; set; }
    public string? Description { get; set; }
}

public class JoinClassroomRequest
{
    public string ClassCode { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
}

public class SubmitClassroomQuizRequest
{
    public List<QuizAnswerDto> Answers { get; set; } = new();
}

public class ClassroomQuizResultResponse
{
    public Guid QuizId { get; set; }
    public Guid StudentId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage { get; set; }
    public List<QuestionResultDto> Results { get; set; } = new();
}

// Stripe Subscription Status (mirror al SubscriptionStatus din backend)
public enum SubscriptionStatus
{
    Pending = 1,
    Active = 2,
    PastDue = 3,
    Canceled = 4,
    Incomplete = 5
}

// Checkout Session DTOs
public class CreateCheckoutSessionRequest
{
    public Guid UserId { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
}

public class CheckoutSessionResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
}

// Wrapper pentru răspunsul paginat de la GET /api/Subscriptions/user/{userId}
// Backend-ul returnează { "value": [...], "Count": N }
public class SubscriptionListResponse
{
    public List<SubscriptionDto> Value { get; set; } = new();
    public int Count { get; set; }
}

// ===== Parent ↔ Child =====
public class InvitationCodeDto
{
    public Guid ParentId { get; set; }
    public string InvitationCode { get; set; } = string.Empty;
}

public class ChildDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LinkParentRequest
{
    public Guid StudentId { get; set; }
    public string InvitationCode { get; set; } = string.Empty;
}

public class LinkParentResponse
{
    public Guid ParentId { get; set; }
    public string ParentFullName { get; set; } = string.Empty;
}

public class ParentInfoDto
{
    public Guid ParentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class StudentGradeDto
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime GradedAt { get; set; }
    public Guid ClassroomId { get; set; }
    public string ClassroomName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
}

public class UpdateClassroomLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Beginner;
}







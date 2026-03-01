namespace AiTutor.Web.Models;

// ─── Auth ───────────────────────────────────────────────────────────────────

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
    public string Role { get; set; } = "Student";
    public int Age { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}

// ─── User ────────────────────────────────────────────────────────────────────

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int Age { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

// ─── Subject ─────────────────────────────────────────────────────────────────

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int LessonCount { get; set; }
}

// ─── Lesson ──────────────────────────────────────────────────────────────────

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int DifficultyLevel { get; set; }
    public string DifficultyName => DifficultyLevel switch
    {
        1 => "Începător",
        2 => "Intermediar",
        3 => "Avansat",
        _ => "Necunoscut"
    };
}

// ─── Progress ────────────────────────────────────────────────────────────────

public class StudentProgressDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public double Score { get; set; }
    public int Attempts { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class CompleteProgressRequest
{
    public string UserId { get; set; } = string.Empty;
    public int LessonId { get; set; }
    public double Score { get; set; }
}

// ─── Quiz ─────────────────────────────────────────────────────────────────────

public class QuizDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int LessonId { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int CorrectOptionIndex { get; set; }
}

// ─── AI Module ───────────────────────────────────────────────────────────────

public class AiExplanationRequest
{
    public string Topic { get; set; } = string.Empty;
    public int Subject { get; set; }
    public int DifficultyLevel { get; set; }
    public int StudentAge { get; set; }
}

public class AiHintRequest
{
    public string Problem { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Subject { get; set; }
}

public class AiExplanationResponse
{
    public string Topic { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public List<string> Examples { get; set; } = new();
    public List<string> KeyPoints { get; set; } = new();
    public string Hint { get; set; } = string.Empty;
}

public class AiProblemsRequest
{
    public string Topic { get; set; } = string.Empty;
    public int Subject { get; set; }
    public int DifficultyLevel { get; set; }
    public int StudentAge { get; set; }
    public int Count { get; set; } = 3;
}

public class AiProblem
{
    public string Statement { get; set; } = string.Empty;
    public List<string> Hints { get; set; } = new();
    public string Solution { get; set; } = string.Empty;
    public bool IsSolutionVisible { get; set; } = false;
}

public class AiProblemsResponse
{
    public string Topic { get; set; } = string.Empty;
    public List<AiProblem> Problems { get; set; } = new();
}

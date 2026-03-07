using FluentValidation;

namespace AiTutor.Application.Features.Quizzes.Commands.SubmitQuiz;

public class SubmitQuizCommandValidator : AbstractValidator<SubmitQuizCommand>
{
    public SubmitQuizCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required.");

        RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("Answers are required.")
            .Must(a => a.Count > 0).WithMessage("At least one answer is required.");
    }
}

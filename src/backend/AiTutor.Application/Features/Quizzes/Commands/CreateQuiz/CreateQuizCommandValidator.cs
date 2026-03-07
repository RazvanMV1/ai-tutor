using FluentValidation;

namespace AiTutor.Application.Features.Quizzes.Commands.CreateQuiz;

public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("Lesson is required.");

        RuleFor(x => x.TimeLimitMinutes)
            .GreaterThan(0).WithMessage("Time limit must be greater than 0.")
            .LessThanOrEqualTo(180).WithMessage("Time limit must not exceed 180 minutes.");
    }
}

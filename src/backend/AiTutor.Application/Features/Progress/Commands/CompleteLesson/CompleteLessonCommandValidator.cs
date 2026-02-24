using FluentValidation;

namespace AiTutor.Application.Features.Progress.Commands.CompleteLesson;

public class CompleteLessonCommandValidator : AbstractValidator<CompleteLessonCommand>
{
    public CompleteLessonCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required.");

        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("Lesson is required.");

        RuleFor(x => x.ScorePercentage)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");
    }
}

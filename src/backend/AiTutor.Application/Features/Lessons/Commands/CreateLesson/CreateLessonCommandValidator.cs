using FluentValidation;

namespace AiTutor.Application.Features.Lessons.Commands.CreateLesson;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.OrderIndex)
            .GreaterThan(0).WithMessage("Order index must be greater than 0.");

        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Subject is required.");
    }
}

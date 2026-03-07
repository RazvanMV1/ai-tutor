using FluentValidation;

namespace AiTutor.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(1000).WithMessage("Question text must not exceed 1000 characters.");

        RuleFor(x => x.CorrectAnswer)
            .NotEmpty().WithMessage("Correct answer is required.");

        RuleFor(x => x.Options)
            .NotEmpty().WithMessage("Options are required.")
            .Must(o => o.Count >= 2).WithMessage("At least 2 options are required.")
            .Must(o => o.Count <= 6).WithMessage("Maximum 6 options allowed.");

        RuleFor(x => x.Points)
            .GreaterThan(0).WithMessage("Points must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Points must not exceed 100.");

        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz is required.");

        RuleFor(x => x)
            .Must(x => x.Options.Contains(x.CorrectAnswer))
            .WithMessage("Correct answer must be one of the options.");
    }
}

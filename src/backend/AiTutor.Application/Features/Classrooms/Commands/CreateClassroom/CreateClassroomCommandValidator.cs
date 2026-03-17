using FluentValidation;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;

public class CreateClassroomCommandValidator : AbstractValidator<CreateClassroomCommand>
{
    public CreateClassroomCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Numele clasei este obligatoriu.")
            .MaximumLength(100).WithMessage("Numele nu poate depăși 100 caractere.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Descrierea nu poate depăși 500 caractere.");
        RuleFor(x => x.TeacherId)
            .NotEmpty().WithMessage("TeacherId este obligatoriu.");
    }
}

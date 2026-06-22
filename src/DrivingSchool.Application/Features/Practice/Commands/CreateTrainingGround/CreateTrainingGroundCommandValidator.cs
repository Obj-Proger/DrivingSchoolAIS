namespace DrivingSchool.Application.Features.Practice.Commands.CreateTrainingGround;

internal sealed class CreateTrainingGroundCommandValidator
    : AbstractValidator<CreateTrainingGroundCommand>
{
    public CreateTrainingGroundCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ground name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Description is not null);
    }
}
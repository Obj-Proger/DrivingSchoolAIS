namespace DrivingSchool.Application.Features.Practice.Commands.UpdateTrainingGround;

internal sealed class UpdateTrainingGroundCommandValidator
    : AbstractValidator<UpdateTrainingGroundCommand>
{
    public UpdateTrainingGroundCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}
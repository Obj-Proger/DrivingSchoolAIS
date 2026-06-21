namespace DrivingSchool.Application.Features.Groups.Commands.CreateGroup;

internal sealed class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Group name is required.")
            .MaximumLength(100).WithMessage("Group name must not exceed 100 characters.");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0).WithMessage("Maximum students must be greater than zero.")
            .LessThanOrEqualTo(50).WithMessage("Maximum students must not exceed 50.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue);
    }
}
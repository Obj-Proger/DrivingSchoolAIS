namespace DrivingSchool.Application.Features.Contracts.Commands.CreateContract;

internal sealed class CreateContractCommandValidator
    : AbstractValidator<CreateContractCommand>
{
    public CreateContractCommandValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Contract number is required.")
            .MaximumLength(50).WithMessage("Contract number must not exceed 50 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.");

        RuleFor(x => x.TotalCost)
            .GreaterThan(0).WithMessage("Total cost must be greater than zero.");
    }
}
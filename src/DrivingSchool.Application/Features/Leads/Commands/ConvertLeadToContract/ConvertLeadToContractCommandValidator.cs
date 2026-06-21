namespace DrivingSchool.Application.Features.Leads.Commands.ConvertLeadToContract;

internal sealed class ConvertLeadToContractCommandValidator
    : AbstractValidator<ConvertLeadToContractCommand>
{
    public ConvertLeadToContractCommandValidator()
    {
        RuleFor(x => x.ContractNumber)
            .NotEmpty().WithMessage("Contract number is required.")
            .MaximumLength(50).WithMessage("Contract number must not exceed 50 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.");

        RuleFor(x => x.TotalCost)
            .GreaterThan(0).WithMessage("Total cost must be greater than zero.");
    }
}
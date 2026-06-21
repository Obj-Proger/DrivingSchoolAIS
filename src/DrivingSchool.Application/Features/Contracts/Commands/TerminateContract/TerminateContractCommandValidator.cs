namespace DrivingSchool.Application.Features.Contracts.Commands.TerminateContract;

internal sealed class TerminateContractCommandValidator
    : AbstractValidator<TerminateContractCommand>
{
    public TerminateContractCommandValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Termination reason is required.")
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");
    }
}
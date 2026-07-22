namespace DrivingSchool.Application.Features.Payments.Commands.CreatePayment;

internal sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.ContractId).NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than zero.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Payment purpose is required.")
            .MaximumLength(500).WithMessage("Purpose must not exceed 500 characters.");

        RuleFor(x => x.Method)
            .IsInEnum().WithMessage("Invalid payment method.");
    }
}
namespace DrivingSchool.Application.Features.Auth.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address is not valid.");
    }
}
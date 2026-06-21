namespace DrivingSchool.Application.Features.Auth.Commands.Login;

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
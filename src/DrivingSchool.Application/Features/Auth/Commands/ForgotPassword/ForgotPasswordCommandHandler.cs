using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Auth.Commands.ForgotPassword;

/// <summary>
/// Handles <see cref="ForgotPasswordCommand"/>.
/// Generates a reset token and sends the password reset email.
/// Always returns success regardless of whether the email is registered.
/// </summary>
internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result> Handle(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByEmailAsync(command.Email.ToLowerInvariant(), cancellationToken);

        // Do not reveal whether the email is registered
        if (user is null || !user.IsActive)
            return Result.Success();

        var resetToken = user.GeneratePasswordResetToken();

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fire-and-forget — email sending should not block the response
        _ = _emailService.SendTemplatedAsync(
            to: user.Email.Value,
            templateName: "PasswordReset",
            variables: new Dictionary<string, string>
            {
                ["UserName"] = user.FullName.FirstName,
                ["ResetToken"] = resetToken
            });

        return Result.Success();
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Auth.Commands.ResetPassword;

/// <summary>
/// Handles <see cref="ResetPasswordCommand"/>.
/// Finds the user by their reset token via the repository,
/// delegates the password change to the domain aggregate,
/// and revokes all existing sessions.
/// </summary>
internal sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByPasswordResetTokenAsync(command.Token, cancellationToken);

        if (user is null)
            return Result.Failure(DomainErrors.Auth.InvalidToken);

        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(
            command.NewPassword, workFactor: 12);

        var resetResult = user.ResetPassword(command.Token, newPasswordHash);
        if (resetResult.IsFailure) return resetResult;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
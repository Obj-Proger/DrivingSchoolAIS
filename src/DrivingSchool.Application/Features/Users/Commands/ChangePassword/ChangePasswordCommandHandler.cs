using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Commands.ChangePassword;

/// <summary>
/// Handles <see cref="ChangePasswordCommand"/>.
/// Verifies the current password, hashes the new one, and revokes all sessions.
/// </summary>
internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound);

        // Verify current password before allowing the change
        if (!BCrypt.Net.BCrypt.Verify(command.CurrentPassword, user.PasswordHash))
            return Result.Failure(DomainErrors.Auth.InvalidCredentials);

        var newHash = BCrypt.Net.BCrypt.HashPassword(command.NewPassword, workFactor: 12);

        user.ChangePassword(newHash);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
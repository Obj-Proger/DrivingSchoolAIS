using System.Security.Cryptography;
using System.Text;
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Handles <see cref="LogoutCommand"/>.
/// Locates the refresh token by its hash and revokes it.
/// Returns success even if the token is not found to prevent information leakage.
/// </summary>
internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(command.RefreshToken)));

        var user = await _unitOfWork.Users
            .GetByRefreshTokenHashAsync(tokenHash, cancellationToken);

        // Silently succeed if the token does not exist or is already revoked
        if (user is null)
            return Result.Success();

        var revokeResult = user.RevokeRefreshToken(tokenHash);

        if (revokeResult.IsFailure)
            return Result.Success(); // Already revoked — treat as success

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
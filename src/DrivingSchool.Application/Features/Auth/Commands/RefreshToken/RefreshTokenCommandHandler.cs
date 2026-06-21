using System.Security.Cryptography;
using System.Text;
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Auth.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using EntityRefreshToken = DrivingSchool.Domain.Entities.RefreshToken;

namespace DrivingSchool.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Handles <see cref="RefreshTokenCommand"/>.
/// Implements refresh token rotation: the submitted token is revoked
/// and a fresh pair is issued.
/// </summary>
internal sealed class RefreshTokenCommandHandler
    : ICommandHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Hash the incoming plain token to look it up
        var tokenHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(command.RefreshToken)));

        // 2. Find the user who owns this token
        var user = await _unitOfWork.Users
            .GetByRefreshTokenHashAsync(tokenHash, cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponseDto>(DomainErrors.Auth.InvalidRefreshToken);

        if (!user.IsActive)
            return Result.Failure<AuthResponseDto>(DomainErrors.Auth.AccountInactive);

        // 3. Generate new token pair
        var (newPlainToken, newTokenHash) = _jwtService.GenerateRefreshToken();

        // 4. Rotate: revoke old, add new
        var revokeResult = user.RevokeRefreshToken(tokenHash, replacedByTokenHash: newTokenHash);
        if (revokeResult.IsFailure)
            return Result.Failure<AuthResponseDto>(revokeResult.Error);

        var newRefreshToken = EntityRefreshToken.Create(
            user.Id,
            newTokenHash,
            expiryDays: int.Parse(_configuration["JWT_REFRESH_DAYS"] ?? "7"),
            createdByIp: command.IpAddress);

        user.AddRefreshToken(newRefreshToken);

        // 5. Persist
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Issue new access token and return
        var accessToken = _jwtService.GenerateAccessToken(user);
        var expiryMinutes = int.Parse(_configuration["JWT_EXPIRY_MINUTES"] ?? "15");

        return Result.Success(new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: newPlainToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(expiryMinutes),
            User: MapToDto(user)));
    }

    private static UserBriefDto MapToDto(User user) => new(
        user.Id,
        user.FullName.DisplayName,
        user.FullName.ShortName,
        user.Email.Value,
        user.Role,
        user.PhotoUrl,
        user.IsEmailConfirmed);
}
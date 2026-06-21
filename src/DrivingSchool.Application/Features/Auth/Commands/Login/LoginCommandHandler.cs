using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Auth.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using EntityRefreshToken = DrivingSchool.Domain.Entities.RefreshToken;

namespace DrivingSchool.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handles <see cref="LoginCommand"/>.
/// Verifies credentials, issues a new token pair, and records the login timestamp.
/// Returns a generic error for both wrong email and wrong password
/// to prevent user enumeration attacks.
/// </summary>
internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Look up user
        var user = await _unitOfWork.Users
            .GetByEmailAsync(command.Email.ToLowerInvariant(), cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponseDto>(DomainErrors.Auth.InvalidCredentials);

        // 2. Verify account status
        if (!user.IsActive)
            return Result.Failure<AuthResponseDto>(DomainErrors.Auth.AccountInactive);

        // 3. Verify password
        var passwordValid = BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash);
        if (!passwordValid)
            return Result.Failure<AuthResponseDto>(DomainErrors.Auth.InvalidCredentials);

        // 4. Issue tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var (plainRefreshToken, tokenHash) = _jwtService.GenerateRefreshToken();

        var refreshToken = EntityRefreshToken.Create(
            user.Id,
            tokenHash,
            expiryDays: int.Parse(_configuration["JWT_REFRESH_DAYS"] ?? "7"),
            createdByIp: command.IpAddress);

        user.AddRefreshToken(refreshToken);
        user.RecordLogin();

        // 5. Persist
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Build response
        var expiryMinutes = int.Parse(_configuration["JWT_EXPIRY_MINUTES"] ?? "15");

        return Result.Success(new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: plainRefreshToken,
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
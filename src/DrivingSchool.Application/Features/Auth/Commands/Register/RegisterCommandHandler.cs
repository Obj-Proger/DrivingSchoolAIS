using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Auth.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using EntityRefreshToken = DrivingSchool.Domain.Entities.RefreshToken;

namespace DrivingSchool.Application.Features.Auth.Commands.Register;

/// <summary>
/// Handles <see cref="RegisterCommand"/>.
/// Validates uniqueness, hashes the password, creates the user, issues tokens,
/// and sends an email confirmation link.
/// </summary>
internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Validate email uniqueness
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure) return Result.Failure<AuthResponseDto>(emailResult.Error);

        var emailExists = await _unitOfWork.Users
            .ExistsByEmailAsync(emailResult.Value.Value, cancellationToken);

        if (emailExists)
            return Result.Failure<AuthResponseDto>(DomainErrors.User.EmailAlreadyTaken);

        // 2. Build value objects
        var fullNameResult = FullName.Create(command.FirstName, command.LastName, command.MiddleName);
        if (fullNameResult.IsFailure) return Result.Failure<AuthResponseDto>(fullNameResult.Error);

        var phoneResult = PhoneNumber.Create(command.Phone);
        if (phoneResult.IsFailure) return Result.Failure<AuthResponseDto>(phoneResult.Error);

        // 3. Hash password 
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password, workFactor: 12);

        // 4. Create user aggregate
        var userResult = User.Create(
            emailResult.Value,
            passwordHash,
            phoneResult.Value,
            fullNameResult.Value,
            command.Role,
            command.Role == UserRole.Student ? null : command.BranchId);

        if (userResult.IsFailure) return Result.Failure<AuthResponseDto>(userResult.Error);

        var user = userResult.Value;

        // 5. Issue tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var (plainRefreshToken, tokenHash) = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = EntityRefreshToken.Create(
            user.Id,
            tokenHash,
            expiryDays: GetRefreshTokenExpiryDays(),
            createdByIp: command.IpAddress);

        user.AddRefreshToken(refreshTokenEntity);

        // 6. Persist
        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Send email confirmation (fire-and-forget — do not block)
        var confirmationToken = user.EmailConfirmationToken;
        if (confirmationToken is not null)
        {
            _ = _emailService.SendTemplatedAsync(
                to: user.Email.Value,
                templateName: "EmailConfirmation",
                variables: new Dictionary<string, string>
                {
                    ["UserName"] = user.FullName.FirstName,
                    ["ConfirmationToken"] = confirmationToken
                });
        }

        // 8. Build response
        var expiryMinutes = int.Parse(_configuration["JWT_EXPIRY_MINUTES"] ?? "15");

        return Result.Success(new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: plainRefreshToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(expiryMinutes),
            User: MapToDto(user)));
    }

    private int GetRefreshTokenExpiryDays()
        => int.Parse(_configuration["JWT_REFRESH_DAYS"] ?? "7");

    private static UserBriefDto MapToDto(User user) => new(
        user.Id,
        user.FullName.DisplayName,
        user.FullName.ShortName,
        user.Email.Value,
        user.Role,
        user.PhotoUrl,
        user.IsEmailConfirmed);
}
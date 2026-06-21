using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Events;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a system user and serves as the authentication aggregate root.
/// Manages credentials, refresh tokens, email confirmation, and profile data.
/// </summary>
public sealed class User : BaseEntity
{
    private readonly List<RefreshToken> _refreshTokens = [];

    private User() { } // Required by EF Core

    /// <summary>Gets the user's validated email address.</summary>
    public Email Email { get; private set; } = null!;

    /// <summary>Gets the BCrypt hash of the user's password.</summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>Gets the user's normalised phone number.</summary>
    public PhoneNumber Phone { get; private set; } = null!;

    /// <summary>Gets the user's full name.</summary>
    public FullName FullName { get; private set; } = null!;

    /// <summary>Gets the user's role, which determines their permissions.</summary>
    public UserRole Role { get; private set; }

    /// <summary>Gets a value indicating whether the account is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets a value indicating whether the email address has been confirmed.</summary>
    public bool IsEmailConfirmed { get; private set; }

    /// <summary>Gets the URL of the user's profile photo, or <c>null</c> if not set.</summary>
    public string? PhotoUrl { get; private set; }

    /// <summary>Gets the UTC timestamp of the user's last successful login, or <c>null</c> if never logged in.</summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// The email confirmation token sent to the user on registration.
    /// Public so the Application layer can include it in the confirmation email.
    /// </summary>
    public string? EmailConfirmationToken { get; private set; }

    /// <summary>UTC expiry of the email confirmation token.</summary>
    public DateTime? EmailConfirmationTokenExpiry { get; private set; }

    /// <summary>
    /// The password reset token sent to the user via email.
    /// Public so the repository can query by this value.
    /// </summary>
    public string? PasswordResetToken { get; private set; }

    /// <summary>UTC expiry of the password reset token.</summary>
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    /// <summary>Gets the collection of refresh tokens associated with this user.</summary>
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    // Factory

    /// <summary>
    /// Creates and initialises a new user account.
    /// Generates an email confirmation token and raises <see cref="UserRegisteredEvent"/>.
    /// </summary>
    /// <param name="email">The validated email address.</param>
    /// <param name="passwordHash">The BCrypt-hashed password.</param>
    /// <param name="phone">The validated phone number.</param>
    /// <param name="fullName">The user's full name.</param>
    /// <param name="role">The role to assign to the new user.</param>
    /// <returns>A successful <see cref="Result{User}"/> containing the new user.</returns>
    public static Result<User> Create(
        Email email,
        string passwordHash,
        PhoneNumber phone,
        FullName fullName,
        UserRole role)
    {
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Phone = phone,
            FullName = fullName,
            Role = role,
            IsActive = true,
            IsEmailConfirmed = false
        };

        user.RefreshEmailConfirmationToken();
        user.RaiseDomainEvent(new UserRegisteredEvent(user.Id, email.Value, fullName.DisplayName));

        return Result.Success(user);
    }

    // Refresh Token Management

    /// <summary>
    /// Adds a new refresh token, removing all expired and revoked tokens first.
    /// </summary>
    /// <param name="token">The refresh token to add.</param>
    public void AddRefreshToken(RefreshToken token)
    {
        _refreshTokens.RemoveAll(t => !t.IsActive);
        _refreshTokens.Add(token);
    }

    /// <summary>
    /// Revokes the refresh token identified by its hash.
    /// </summary>
    /// <param name="tokenHash">The SHA-256 hash of the token to revoke.</param>
    /// <param name="replacedByTokenHash">
    /// The hash of the replacement token during rotation, or <c>null</c>.
    /// </param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Auth.InvalidRefreshToken"/>
    /// or <see cref="DomainErrors.Auth.TokenAlreadyRevoked"/>.
    /// </returns>
    public Result RevokeRefreshToken(string tokenHash, string? replacedByTokenHash = null)
    {
        var token = _refreshTokens.SingleOrDefault(t => t.TokenHash == tokenHash);

        if (token is null)
            return Result.Failure(DomainErrors.Auth.InvalidRefreshToken);

        if (token.IsRevoked)
            return Result.Failure(DomainErrors.Auth.TokenAlreadyRevoked);

        token.Revoke(replacedByTokenHash);
        return Result.Success();
    }

    /// <summary>
    /// Revokes all refresh tokens associated with this user.
    /// Called when the password is changed to invalidate all existing sessions.
    /// </summary>
    public void RevokeAllRefreshTokens()
        => _refreshTokens.ForEach(t => t.Revoke());

    // Email Confirmation

    /// <summary>
    /// Confirms the user's email address using the provided token.
    /// </summary>
    /// <param name="token">The confirmation token sent to the user's email.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Auth.EmailAlreadyConfirmed"/>,
    /// <see cref="DomainErrors.Auth.InvalidToken"/>, or <see cref="DomainErrors.Auth.TokenExpired"/>.
    /// </returns>
    public Result ConfirmEmail(string token)
    {
        if (IsEmailConfirmed)
            return Result.Failure(DomainErrors.Auth.EmailAlreadyConfirmed);

        if (EmailConfirmationToken != token)
            return Result.Failure(DomainErrors.Auth.InvalidToken);

        if (DateTime.UtcNow > EmailConfirmationTokenExpiry)
            return Result.Failure(DomainErrors.Auth.TokenExpired);

        IsEmailConfirmed = true;
        EmailConfirmationToken = null;
        EmailConfirmationTokenExpiry = null;

        return Result.Success();
    }

    /// <summary>
    /// Generates a new email confirmation token, overwriting any existing one.
    /// The token is valid for 24 hours.
    /// </summary>
    /// <returns>The plain confirmation token to include in the email link.</returns>
    public string RefreshEmailConfirmationToken()
    {
        var token = GenerateSecureToken();
        EmailConfirmationToken = token;
        EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);
        return token;
    }

    // Password Reset

    /// <summary>
    /// Generates a password reset token valid for one hour.
    /// </summary>
    /// <returns>The plain reset token to include in the password reset link.</returns>
    public string GeneratePasswordResetToken()
    {
        var token = GenerateSecureToken();
        PasswordResetToken = token;
        PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
        return token;
    }

    /// <summary>
    /// Resets the user's password using a valid reset token.
    /// Revokes all existing refresh tokens on success.
    /// </summary>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPasswordHash">The BCrypt hash of the new password.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Auth.InvalidToken"/>
    /// or <see cref="DomainErrors.Auth.TokenExpired"/>.
    /// </returns>
    public Result ResetPassword(string token, string newPasswordHash)
    {
        if (PasswordResetToken != token)
            return Result.Failure(DomainErrors.Auth.InvalidToken);

        if (DateTime.UtcNow > PasswordResetTokenExpiry)
            return Result.Failure(DomainErrors.Auth.TokenExpired);

        PasswordHash = newPasswordHash;
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
        RevokeAllRefreshTokens();

        return Result.Success();
    }

    // Profile Management

    /// <summary>
    /// Updates the user's profile information.
    /// </summary>
    /// <param name="fullName">The updated full name.</param>
    /// <param name="phone">The updated phone number.</param>
    /// <param name="photoUrl">The URL of the updated profile photo, or <c>null</c> to clear it.</param>
    public void UpdateProfile(FullName fullName, PhoneNumber phone, string? photoUrl)
    {
        FullName = fullName;
        Phone = phone;
        PhotoUrl = photoUrl;
    }

    /// <summary>
    /// Changes the user's password and revokes all existing refresh tokens,
    /// forcing re-authentication on all devices.
    /// </summary>
    /// <param name="newPasswordHash">The BCrypt hash of the new password.</param>
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        RevokeAllRefreshTokens();
    }

    // Status & Role

    /// <summary>
    /// Deactivates the user account, preventing further logins.
    /// Raises <see cref="UserDeactivatedEvent"/>.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.User.AlreadyInactive"/>.
    /// </returns>
    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure(DomainErrors.User.AlreadyInactive);

        IsActive = false;
        RevokeAllRefreshTokens();
        RaiseDomainEvent(new UserDeactivatedEvent(Id));

        return Result.Success();
    }

    /// <summary>Assigns a new role to the user.</summary>
    /// <param name="role">The role to assign.</param>
    public void AssignRole(UserRole role) => Role = role;

    /// <summary>Records the current UTC timestamp as the user's last login time.</summary>
    public void RecordLogin() => LastLoginAt = DateTime.UtcNow;

    // Private Helpers

    /// <summary>
    /// Generates a cryptographically secure random token as a 64-character hexadecimal string.
    /// </summary>
    private static string GenerateSecureToken()
        => Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
}
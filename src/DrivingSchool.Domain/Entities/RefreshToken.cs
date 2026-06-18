using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a refresh token belonging to a <see cref="User"/>.
/// Only the SHA-256 hash of the token is stored; the plain value is sent to the client.
/// </summary>
public sealed class RefreshToken : BaseEntity
{
    private RefreshToken() { } // Required by EF Core

    /// <summary>Gets the identifier of the user this token belongs to.</summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the SHA-256 hash of the plain refresh token.
    /// The plain token is only ever sent to the client and never persisted.
    /// </summary>
    public string TokenHash { get; private set; } = string.Empty;

    /// <summary>Gets the UTC timestamp when this token expires.</summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>Gets the IP address of the client that created this token.</summary>
    public string CreatedByIp { get; private set; } = string.Empty;

    /// <summary>Gets the UTC timestamp when this token was revoked, or <c>null</c> if still active.</summary>
    public DateTime? RevokedAt { get; private set; }

    /// <summary>
    /// Gets the hash of the token that replaced this one during rotation,
    /// or <c>null</c> if this token was not rotated.
    /// </summary>
    public string? ReplacedByTokenHash { get; private set; }

    /// <summary>Gets a value indicating whether this token has passed its expiry date.</summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    /// <summary>Gets a value indicating whether this token has been explicitly revoked.</summary>
    public bool IsRevoked => RevokedAt.HasValue;

    /// <summary>Gets a value indicating whether this token is valid and can be used.</summary>
    public bool IsActive => !IsRevoked && !IsExpired;

    /// <summary>
    /// Creates a new refresh token for the specified user.
    /// </summary>
    /// <param name="userId">The owner's identifier.</param>
    /// <param name="tokenHash">The SHA-256 hash of the plain token.</param>
    /// <param name="expiryDays">Number of days until the token expires.</param>
    /// <param name="createdByIp">The IP address of the requesting client.</param>
    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        int expiryDays,
        string createdByIp)
        => new()
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
            CreatedByIp = createdByIp
        };

    /// <summary>
    /// Revokes this token, optionally recording the hash of the token that replaced it.
    /// </summary>
    /// <param name="replacedByTokenHash">
    /// The hash of the new token issued during rotation, or <c>null</c> if not rotated.
    /// </param>
    public void Revoke(string? replacedByTokenHash = null)
    {
        RevokedAt = DateTime.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}
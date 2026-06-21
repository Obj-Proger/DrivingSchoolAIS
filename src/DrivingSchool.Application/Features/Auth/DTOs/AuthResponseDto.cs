namespace DrivingSchool.Application.Features.Auth.DTOs;

/// <summary>
/// Returned by the login and refresh-token endpoints.
/// Contains the access token, refresh token, and a brief user snapshot.
/// </summary>
public sealed record AuthResponseDto(
    /// <summary>The signed JWT access token. Expires in <c>JWT_EXPIRY_MINUTES</c> minutes.</summary>
    string AccessToken,

    /// <summary>
    /// The opaque refresh token (plain value, not the stored hash).
    /// Store in a secure HttpOnly cookie or platform secure storage.
    /// </summary>
    string RefreshToken,

    /// <summary>UTC timestamp when the access token expires.</summary>
    DateTime ExpiresAt,

    /// <summary>Brief identity information about the authenticated user.</summary>
    UserBriefDto User);
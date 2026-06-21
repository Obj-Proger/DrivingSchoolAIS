using System.Security.Claims;

namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Generates and validates JSON Web Tokens used for API authentication.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a signed access token containing the user's identity and role claims.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>A signed JWT string.</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a cryptographically secure refresh token.
    /// </summary>
    /// <returns>
    /// A tuple containing the plain token (sent to the client)
    /// and its SHA-256 hash (stored in the database).
    /// </returns>
    (string PlainToken, string TokenHash) GenerateRefreshToken();

    /// <summary>
    /// Validates a JWT string and returns its claims principal.
    /// </summary>
    /// <param name="token">The JWT string to validate.</param>
    /// <returns>
    /// The <see cref="ClaimsPrincipal"/> if the token is valid;
    /// <c>null</c> if the token is expired, malformed, or has an invalid signature.
    /// </returns>
    ClaimsPrincipal? ValidateToken(string token);
}
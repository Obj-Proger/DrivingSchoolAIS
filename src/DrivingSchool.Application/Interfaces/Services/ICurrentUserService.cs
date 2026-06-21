namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Provides identity information about the currently authenticated user,
/// extracted from the JWT claims in the active HTTP context.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>Gets the unique identifier of the current user.</summary>
    Guid UserId { get; }

    /// <summary>Gets the email address of the current user.</summary>
    string Email { get; }

    /// <summary>Gets the role of the current user.</summary>
    UserRole Role { get; }

    /// <summary>Gets the IP address of the current request.</summary>
    string IpAddress { get; }

    /// <summary>Gets a value indicating whether the current request is authenticated.</summary>
    bool IsAuthenticated { get; }

    /// <summary>Returns <c>true</c> if the current user has the specified role.</summary>
    bool IsInRole(UserRole role);
}
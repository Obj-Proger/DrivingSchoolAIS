namespace DrivingSchool.Application.Features.Users.DTOs;

/// <summary>
/// Extended user profile returned for the currently authenticated user.
/// Includes the user's own data and a summary of their contracts.
/// </summary>
public sealed record UserProfileDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    string DisplayName,
    string ShortName,
    string Email,
    string Phone,
    UserRole Role,
    string? PhotoUrl,
    bool IsActive,
    bool IsEmailConfirmed,
    DateTime? LastLoginAt,
    DateTime CreatedAt,

    /// <summary>
    /// Identifiers of all contracts held by this user.
    /// Relevant for students who may have multiple contracts.
    /// </summary>
    IReadOnlyList<Guid> ContractIds);
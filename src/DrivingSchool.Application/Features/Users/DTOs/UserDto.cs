namespace DrivingSchool.Application.Features.Users.DTOs;

/// <summary>
/// Full user profile data returned by user management endpoints.
/// </summary>
public sealed record UserDto(
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
    Guid? BranchId);
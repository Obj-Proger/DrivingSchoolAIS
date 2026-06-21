namespace DrivingSchool.Application.Features.Auth.DTOs;

/// <summary>
/// A lightweight snapshot of user identity included in every authentication response.
/// </summary>
public sealed record UserBriefDto(
    Guid Id,
    string FullName,
    string ShortName,
    string Email,
    UserRole Role,
    string? PhotoUrl,
    bool IsEmailConfirmed);
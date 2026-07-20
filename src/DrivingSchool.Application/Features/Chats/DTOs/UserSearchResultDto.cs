namespace DrivingSchool.Application.Features.Chats.DTOs;

/// <summary>A lightweight user reference used when picking a recipient for a new chat.</summary>
public sealed record UserSearchResultDto(
    Guid Id,
    string Name,
    string? PhotoUrl,
    UserRole Role);
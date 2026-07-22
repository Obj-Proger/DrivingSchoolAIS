namespace DrivingSchool.Application.Features.Notifications.DTOs;

/// <summary>Represents a single in-app notification.</summary>
public sealed record NotificationDto(
    Guid Id,
    string Title,
    string Body,
    NotificationType Type,
    bool IsRead,
    string? ActionUrl,
    DateTime CreatedAt);
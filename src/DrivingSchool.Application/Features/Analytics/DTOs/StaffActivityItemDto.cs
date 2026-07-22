namespace DrivingSchool.Application.Features.Analytics.DTOs;

/// <summary>Task activity statistics for a single staff member within a date range.</summary>
public sealed record StaffActivityItemDto(
    Guid UserId,
    string UserName,
    UserRole Role,
    int TasksAssigned,
    int TasksCompleted,
    int TasksOverdue);
namespace DrivingSchool.Application.Features.Tasks.DTOs;

/// <summary>
/// Represents a staff task in list and detail views.
/// </summary>
public sealed record StaffTaskDto(
    Guid Id,
    string Title,
    string? Description,
    Guid AssignedToId,
    string AssignedToName,
    Guid CreatedById,
    string CreatedByName,
    DateTime? DueDate,
    TaskItemStatus Status,
    TaskPriority Priority,
    bool IsRecurring,
    IReadOnlyList<DayOfWeek> RecurringDays,
    string? LinkedEntityType,
    Guid? LinkedEntityId,
    DateTime CreatedAt);
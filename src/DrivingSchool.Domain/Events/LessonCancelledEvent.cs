using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a theory lesson is cancelled.
/// Triggers notifications to all students in the affected group.
/// </summary>
/// <param name="LessonId">The identifier of the cancelled lesson.</param>
/// <param name="GroupId">The group whose students must be notified.</param>
/// <param name="ScheduledAt">The original scheduled time, included in notifications.</param>
/// <param name="Reason">The reason provided by the teacher for cancellation.</param>
public sealed record LessonCancelledEvent(
    Guid LessonId,
    Guid GroupId,
    DateTime ScheduledAt,
    string Reason) : IDomainEvent;
namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a new exam event is scheduled for a training group.
/// Triggers notifications to all students and the responsible teacher.
/// </summary>
/// <param name="ExamEventId">The identifier of the newly scheduled exam.</param>
/// <param name="GroupId">The group whose members must be notified.</param>
/// <param name="ScheduledAt">The UTC date and time of the exam.</param>
/// <param name="Type">Whether the exam is a theory or practical assessment.</param>
public sealed record ExamScheduledEvent(
    Guid ExamEventId,
    Guid GroupId,
    DateTime ScheduledAt,
    ExamType Type) : IDomainEvent;
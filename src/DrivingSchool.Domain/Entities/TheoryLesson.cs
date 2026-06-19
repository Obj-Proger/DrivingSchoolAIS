using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a scheduled theory lesson delivered to a training group.
/// The lesson aggregate manages its materials and the attendance records
/// of all students who were expected to attend.
/// </summary>
public sealed class TheoryLesson : BaseEntity
{
    private readonly List<LessonMaterial> _materials = [];
    private readonly List<Attendance> _attendance = [];

    private TheoryLesson() { } // Required by EF Core

    /// <summary>Gets the identifier of the group this lesson is scheduled for.</summary>
    public Guid GroupId { get; private set; }

    /// <summary>Gets the identifier of the teacher delivering this lesson.</summary>
    public Guid TeacherId { get; private set; }

    /// <summary>Gets the identifier of the course topic covered in this lesson.</summary>
    public Guid TopicId { get; private set; }

    /// <summary>Gets the lesson title.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets an optional description or lesson plan notes.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets the UTC timestamp when the lesson is scheduled to begin.</summary>
    public DateTime ScheduledAt { get; private set; }

    /// <summary>Gets the planned duration of the lesson in minutes.</summary>
    public int DurationMinutes { get; private set; }

    /// <summary>
    /// Gets the room number or online meeting URL where the lesson will take place.
    /// </summary>
    public string RoomOrLink { get; private set; } = string.Empty;

    /// <summary>Gets the current status of the lesson.</summary>
    public LessonStatus Status { get; private set; }

    /// <summary>
    /// Gets the reason for cancellation, or <c>null</c> if the lesson was not cancelled.
    /// </summary>
    public string? CancellationReason { get; private set; }

    /// <summary>Gets the materials attached to this lesson.</summary>
    public IReadOnlyList<LessonMaterial> Materials => _materials.AsReadOnly();

    /// <summary>Gets the attendance records for this lesson.</summary>
    public IReadOnlyList<Attendance> AttendanceRecords => _attendance.AsReadOnly();

    // Factory

    /// <summary>
    /// Schedules a new theory lesson for a training group.
    /// </summary>
    /// <param name="groupId">The target group identifier.</param>
    /// <param name="teacherId">The delivering teacher's identifier.</param>
    /// <param name="topicId">The course topic to be covered.</param>
    /// <param name="title">The lesson title.</param>
    /// <param name="scheduledAt">The UTC start time.</param>
    /// <param name="durationMinutes">The planned duration in minutes.</param>
    /// <param name="roomOrLink">The room or online meeting URL.</param>
    /// <param name="description">An optional description or lesson plan.</param>
    /// <returns>
    /// A successful <see cref="Result{TheoryLesson}"/>,
    /// or a failure with <see cref="DomainErrors.TheoryLesson.InvalidDuration"/>
    /// or <see cref="DomainErrors.TheoryLesson.PastScheduledTime"/>.
    /// </returns>
    public static Result<TheoryLesson> Create(
        Guid groupId,
        Guid teacherId,
        Guid topicId,
        string title,
        DateTime scheduledAt,
        int durationMinutes,
        string roomOrLink,
        string? description = null)
    {
        if (durationMinutes <= 0)
            return Result.Failure<TheoryLesson>(DomainErrors.TheoryLesson.InvalidDuration);

        if (scheduledAt <= DateTime.UtcNow)
            return Result.Failure<TheoryLesson>(DomainErrors.TheoryLesson.PastScheduledTime);

        return Result.Success(new TheoryLesson
        {
            GroupId = groupId,
            TeacherId = teacherId,
            TopicId = topicId,
            Title = title,
            ScheduledAt = scheduledAt,
            DurationMinutes = durationMinutes,
            RoomOrLink = roomOrLink,
            Description = description,
            Status = LessonStatus.Scheduled
        });
    }

    // Status Transitions

    /// <summary>
    /// Marks the lesson as completed.
    /// A lesson can only be completed while in the <see cref="LessonStatus.Scheduled"/> state.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.CannotComplete"/>.
    /// </returns>
    public Result Complete()
    {
        if (Status != LessonStatus.Scheduled)
            return Result.Failure(DomainErrors.TheoryLesson.CannotComplete);

        Status = LessonStatus.Completed;
        return Result.Success();
    }

    /// <summary>
    /// Cancels the lesson with a mandatory reason.
    /// Raises <see cref="LessonCancelledEvent"/> to notify the group.
    /// </summary>
    /// <param name="reason">The reason for cancellation. Required.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.CannotCancel"/>.
    /// </returns>
    public Result Cancel(string reason)
    {
        if (Status != LessonStatus.Scheduled)
            return Result.Failure(DomainErrors.TheoryLesson.CannotCancel);

        Status = LessonStatus.Cancelled;
        CancellationReason = reason;

        RaiseDomainEvent(new LessonCancelledEvent(Id, GroupId, ScheduledAt, reason));

        return Result.Success();
    }

    /// <summary>
    /// Reschedules the lesson to a new start time.
    /// Only applicable while the lesson is in the <see cref="LessonStatus.Scheduled"/> state.
    /// </summary>
    /// <param name="newScheduledAt">The new UTC start time. Must be in the future.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.CannotReschedule"/>
    /// or <see cref="DomainErrors.TheoryLesson.PastScheduledTime"/>.
    /// </returns>
    public Result Reschedule(DateTime newScheduledAt)
    {
        if (Status != LessonStatus.Scheduled)
            return Result.Failure(DomainErrors.TheoryLesson.CannotReschedule);

        if (newScheduledAt <= DateTime.UtcNow)
            return Result.Failure(DomainErrors.TheoryLesson.PastScheduledTime);

        ScheduledAt = newScheduledAt;
        return Result.Success();
    }

    // Materials

    /// <summary>
    /// Attaches a material to this lesson.
    /// </summary>
    /// <param name="material">The material to attach.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.IsCancelled"/>.
    /// </returns>
    public Result AddMaterial(LessonMaterial material)
    {
        if (Status == LessonStatus.Cancelled)
            return Result.Failure(DomainErrors.TheoryLesson.IsCancelled);

        _materials.Add(material);
        return Result.Success();
    }

    /// <summary>
    /// Removes a material from this lesson.
    /// </summary>
    /// <param name="materialId">The identifier of the material to remove.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.MaterialNotFound"/>.
    /// </returns>
    public Result RemoveMaterial(Guid materialId)
    {
        var material = _materials.FirstOrDefault(m => m.Id == materialId);

        if (material is null)
            return Result.Failure(DomainErrors.TheoryLesson.MaterialNotFound);

        _materials.Remove(material);
        return Result.Success();
    }

    // Attendance

    /// <summary>
    /// Records or updates the attendance of a student identified by their contract.
    /// If a record for the contract already exists it is updated in place;
    /// otherwise a new record is created.
    /// </summary>
    /// <param name="contractId">The contract identifier of the student.</param>
    /// <param name="isPresent">Whether the student was present.</param>
    /// <param name="note">An optional teacher note about the student's attendance.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.IsCancelled"/>.
    /// </returns>
    public Result MarkAttendance(Guid contractId, bool isPresent, string? note = null)
    {
        if (Status == LessonStatus.Cancelled)
            return Result.Failure(DomainErrors.TheoryLesson.IsCancelled);

        var existing = _attendance.FirstOrDefault(a => a.ContractId == contractId);

        if (existing is not null)
        {
            existing.Update(isPresent, note);
        }
        else
        {
            _attendance.Add(Attendance.Create(Id, contractId, isPresent, note));
        }

        return Result.Success();
    }

    // Updates

    /// <summary>Updates the lesson's editable fields.</summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TheoryLesson.CannotReschedule"/>
    /// if the lesson is not in the <see cref="LessonStatus.Scheduled"/> state.
    /// </returns>
    public Result Update(
        string title,
        string? description,
        DateTime scheduledAt,
        int durationMinutes,
        string roomOrLink)
    {
        if (Status != LessonStatus.Scheduled)
            return Result.Failure(DomainErrors.TheoryLesson.CannotReschedule);

        Title = title;
        Description = description;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        RoomOrLink = roomOrLink;

        return Result.Success();
    }
}
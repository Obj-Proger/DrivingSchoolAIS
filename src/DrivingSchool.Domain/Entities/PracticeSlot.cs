using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a time slot created by an instructor and made available
/// for students to book a practical driving lesson.
/// </summary>
public sealed class PracticeSlot : BaseEntity
{
    private PracticeSlot() { } // Required by EF Core

    /// <summary>Gets the identifier of the instructor who owns this slot.</summary>
    public Guid InstructorId { get; private set; }

    /// <summary>
    /// Gets the identifier of the vehicle assigned to this slot,
    /// or <c>null</c> if not yet assigned.
    /// </summary>
    public Guid? VehicleId { get; private set; }

    /// <summary>
    /// Gets the identifier of the default training ground for this slot,
    /// or <c>null</c> if the student may choose.
    /// </summary>
    public Guid? DefaultTrainingGroundId { get; private set; }

    /// <summary>Gets the UTC timestamp when the slot begins.</summary>
    public DateTime StartDateTime { get; private set; }

    /// <summary>Gets the UTC timestamp when the slot ends.</summary>
    public DateTime EndDateTime { get; private set; }

    /// <summary>Gets the calculated duration of the slot in minutes.</summary>
    public int DurationMinutes => (int)(EndDateTime - StartDateTime).TotalMinutes;

    /// <summary>Gets the type of activity planned for this slot.</summary>
    public SlotType Type { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the student may choose their preferred
    /// training ground when booking this slot.
    /// </summary>
    public bool IsOpenForStudentGroundChoice { get; private set; }

    /// <summary>Gets the current availability status of the slot.</summary>
    public SlotStatus Status { get; private set; }

    /// <summary>Gets an optional internal note from the instructor.</summary>
    public string? Note { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this slot can currently be booked by a student.
    /// </summary>
    public bool CanBeBooked =>
        Status == SlotStatus.Available &&
        Type != SlotType.Personal &&
        StartDateTime > DateTime.UtcNow;

    // Factory

    /// <summary>
    /// Creates a new available practice slot.
    /// </summary>
    /// <param name="instructorId">The owning instructor's identifier.</param>
    /// <param name="startDateTime">The UTC start time.</param>
    /// <param name="endDateTime">The UTC end time. Must be after <paramref name="startDateTime"/>.</param>
    /// <param name="type">The type of the slot.</param>
    /// <param name="vehicleId">The assigned vehicle (optional).</param>
    /// <param name="defaultTrainingGroundId">The default training ground (optional).</param>
    /// <param name="isOpenForStudentGroundChoice">Whether students may choose a ground.</param>
    /// <param name="note">An optional internal note.</param>
    /// <returns>
    /// A successful <see cref="Result{PracticeSlot}"/>,
    /// or a failure with <see cref="DomainErrors.PracticeSlot.InvalidTimeRange"/>.
    /// </returns>
    public static Result<PracticeSlot> Create(
        Guid instructorId,
        DateTime startDateTime,
        DateTime endDateTime,
        SlotType type,
        Guid? vehicleId = null,
        Guid? defaultTrainingGroundId = null,
        bool isOpenForStudentGroundChoice = false,
        string? note = null)
    {
        if (endDateTime <= startDateTime)
            return Result.Failure<PracticeSlot>(DomainErrors.PracticeSlot.InvalidTimeRange);

        return Result.Success(new PracticeSlot
        {
            InstructorId = instructorId,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            Type = type,
            VehicleId = vehicleId,
            DefaultTrainingGroundId = defaultTrainingGroundId,
            IsOpenForStudentGroundChoice = isOpenForStudentGroundChoice,
            Note = note,
            Status = SlotStatus.Available
        });
    }

    // Status Transitions

    /// <summary>
    /// Marks the slot as booked. Called when a student creates a booking.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeSlot.NotAvailable"/>.
    /// </returns>
    public Result Book()
    {
        if (!CanBeBooked)
            return Result.Failure(DomainErrors.PracticeSlot.NotAvailable);

        Status = SlotStatus.Booked;
        return Result.Success();
    }

    /// <summary>
    /// Releases the slot back to available. Called when a booking is cancelled.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeSlot.NotBooked"/>.
    /// </returns>
    public Result Release()
    {
        if (Status != SlotStatus.Booked)
            return Result.Failure(DomainErrors.PracticeSlot.NotBooked);

        Status = SlotStatus.Available;
        return Result.Success();
    }

    /// <summary>Cancels the slot. All statuses except Completed can be cancelled.</summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeSlot.AlreadyCompleted"/>.
    /// </returns>
    public Result Cancel()
    {
        if (Status == SlotStatus.Completed)
            return Result.Failure(DomainErrors.PracticeSlot.AlreadyCompleted);

        Status = SlotStatus.Cancelled;
        return Result.Success();
    }

    /// <summary>Marks the slot as completed after the lesson has been delivered.</summary>
    public void Complete() => Status = SlotStatus.Completed;

    // Updates

    /// <summary>
    /// Updates the slot's schedule and assigned resources.
    /// Only applicable while the slot is <see cref="SlotStatus.Available"/>.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeSlot.NotAvailable"/>.
    /// </returns>
    public Result Update(
        DateTime startDateTime,
        DateTime endDateTime,
        Guid? vehicleId,
        Guid? defaultTrainingGroundId,
        string? note)
    {
        if (Status != SlotStatus.Available)
            return Result.Failure(DomainErrors.PracticeSlot.NotAvailable);

        if (endDateTime <= startDateTime)
            return Result.Failure(DomainErrors.PracticeSlot.InvalidTimeRange);

        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        VehicleId = vehicleId;
        DefaultTrainingGroundId = defaultTrainingGroundId;
        Note = note;

        return Result.Success();
    }
}
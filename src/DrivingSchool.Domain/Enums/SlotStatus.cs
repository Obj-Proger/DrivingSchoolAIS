namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the current availability state of a practice slot.
/// </summary>
public enum SlotStatus
{
    /// <summary>The slot is open and can be booked by a student.</summary>
    Available = 1,

    /// <summary>The slot has been reserved by a student.</summary>
    Booked = 2,

    /// <summary>The slot was cancelled by the instructor before it took place.</summary>
    Cancelled = 3,

    /// <summary>The lesson has been delivered and the slot is closed.</summary>
    Completed = 4
}
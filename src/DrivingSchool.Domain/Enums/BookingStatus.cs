namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a student's practice booking.
/// </summary>
public enum BookingStatus
{
    /// <summary>The booking is active and the lesson is upcoming.</summary>
    Confirmed = 1,

    /// <summary>The lesson has been delivered and evaluated by the instructor.</summary>
    Completed = 2,

    /// <summary>The booking was cancelled by the student.</summary>
    CancelledByStudent = 3,

    /// <summary>The booking was cancelled by the instructor.</summary>
    CancelledByInstructor = 4
}
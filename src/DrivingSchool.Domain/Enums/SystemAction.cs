namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Identifies the domain event type encoded in a system message.
/// The client uses this value to render the appropriate action button
/// and navigate to the relevant entity on tap.
/// </summary>
public enum SystemAction
{
    /// <summary>A practice booking was confirmed. Action: open booking details.</summary>
    BookingCreated = 1,

    /// <summary>A practice booking was cancelled. Action: open schedule.</summary>
    BookingCancelled = 2,

    /// <summary>A practice lesson was completed. Action: submit rating.</summary>
    BookingCompleted = 3,

    /// <summary>A theory lesson was scheduled. Action: open lesson details.</summary>
    LessonScheduled = 4,

    /// <summary>A theory lesson was cancelled. Action: open schedule.</summary>
    LessonCancelled = 5,

    /// <summary>An exam has been scheduled. Action: open exam details.</summary>
    ExamScheduled = 6,

    /// <summary>A payment was recorded. Action: open payment history.</summary>
    PaymentReceived = 7,

    /// <summary>A staff task was assigned. Action: open task details.</summary>
    TaskAssigned = 8,

    /// <summary>A new contract was signed. Action: open contract details.</summary>
    ContractSigned = 9
}
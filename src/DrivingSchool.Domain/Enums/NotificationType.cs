namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Categorises a notification for routing, display and filtering purposes.
/// </summary>
public enum NotificationType
{
    /// <summary>Reminder sent to a student before an upcoming theory lesson.</summary>
    LessonReminder = 1,

    /// <summary>Confirmation sent to the instructor when a student books a slot.</summary>
    PracticeBooked = 2,

    /// <summary>Confirmation sent to the student after their booking is created.</summary>
    PracticeConfirmed = 3,

    /// <summary>Notification sent when a practice booking is cancelled.</summary>
    PracticeCancelled = 4,

    /// <summary>Notification sent to the student after the instructor completes a lesson.</summary>
    PracticeCompleted = 5,

    /// <summary>Request sent to the student asking them to rate a completed lesson.</summary>
    RatingRequest = 6,

    /// <summary>Notification sent to a group when an exam is scheduled.</summary>
    ExamScheduled = 7,

    /// <summary>Notification sent to the student after their exam result is recorded.</summary>
    ExamResult = 8,

    /// <summary>Confirmation sent to the student when a payment is confirmed.</summary>
    PaymentReceived = 9,

    /// <summary>Alert sent to a manager when a student's debt exceeds a threshold.</summary>
    DebtAlert = 10,

    /// <summary>Alert sent to managers when a student submits a rating below 4.</summary>
    LowQualityAlert = 11,

    /// <summary>Notification for a new chat message when the recipient is offline.</summary>
    NewMessage = 12,

    /// <summary>Notification sent to a staff member when a task is assigned to them.</summary>
    TaskAssigned = 13,

    /// <summary>Alert sent to the assignee when a task deadline has passed.</summary>
    TaskOverdue = 14,

    /// <summary>A general system-generated notification.</summary>
    SystemMessage = 15,

    /// <summary>Notification sent to a group's students when a theory lesson is cancelled.</summary>
    LessonCancelled = 16,

    /// <summary>Notification sent to the student when a payment is refunded.</summary>
    PaymentRefunded = 17
}
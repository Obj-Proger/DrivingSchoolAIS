namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the current state of a theory lesson.
/// </summary>
public enum LessonStatus
{
    /// <summary>The lesson is planned and has not yet taken place.</summary>
    Scheduled = 1,

    /// <summary>The lesson has been delivered and attendance has been recorded.</summary>
    Completed = 2,

    /// <summary>The lesson was cancelled before it could take place.</summary>
    Cancelled = 3
}
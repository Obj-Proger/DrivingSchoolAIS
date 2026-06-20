namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the current state of a student's test session.
/// </summary>
public enum SessionStatus
{
    /// <summary>The student is actively answering questions.</summary>
    InProgress = 1,

    /// <summary>The student submitted their answers before the time limit expired.</summary>
    Completed = 2,

    /// <summary>The time limit elapsed before the student finished all questions.</summary>
    TimedOut = 3
}
namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines how a test session behaves when the student gives an incorrect answer.
/// Configured per <see cref="DrivingSchool.Domain.Entities.TestTemplate"/>.
/// </summary>
public enum ErrorHandlingMode
{
    /// <summary>
    /// The session ends immediately on the first incorrect answer.
    /// Used for the strictest examination mode.
    /// </summary>
    StopOnError = 1,

    /// <summary>
    /// Extra questions and time are added after each incorrect answer,
    /// giving the student an opportunity to compensate.
    /// The number of extras is configured in the template.
    /// </summary>
    AddQuestions = 2,

    /// <summary>
    /// Incorrect answers are recorded but have no impact on session flow.
    /// The student continues through all questions normally.
    /// </summary>
    Ignore = 3
}
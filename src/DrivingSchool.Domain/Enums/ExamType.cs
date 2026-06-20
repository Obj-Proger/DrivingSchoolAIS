namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines whether an exam event tests theoretical knowledge or practical driving skills.
/// </summary>
public enum ExamType
{
    /// <summary>A written or computer-based theory knowledge test.</summary>
    Theory = 1,

    /// <summary>An on-road practical driving assessment.</summary>
    Practice = 2
}
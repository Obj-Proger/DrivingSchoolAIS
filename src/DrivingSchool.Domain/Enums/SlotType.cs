namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the purpose of a practice slot created by an instructor.
/// </summary>
public enum SlotType
{
    /// <summary>A regular practical driving lesson.</summary>
    Driving = 1,

    /// <summary>An internal practical examination session.</summary>
    Exam = 2,

    /// <summary>
    /// Personal time blocked by the instructor.
    /// Not visible to students and cannot be booked.
    /// </summary>
    Personal = 3,

    /// <summary>A slot type that does not fit the other categories.</summary>
    Other = 4
}
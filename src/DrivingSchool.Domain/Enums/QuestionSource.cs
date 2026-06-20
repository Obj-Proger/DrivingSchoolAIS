namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Identifies the origin of a question in the bank.
/// </summary>
public enum QuestionSource
{
    /// <summary>
    /// An official question from the ГИБДД examination database.
    /// </summary>
    Official = 1,

    /// <summary>
    /// A question created by the school's teaching staff.
    /// </summary>
    Custom = 2
}
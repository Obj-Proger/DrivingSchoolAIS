namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the purpose and rules of a test configured through a <see cref="DrivingSchool.Domain.Entities.TestTemplate"/>.
/// </summary>
public enum TestType
{
    /// <summary>
    /// An unlimited practice test with no formal consequences.
    /// Students may attempt it as many times as they wish.
    /// </summary>
    Practice = 1,

    /// <summary>
    /// An intermediate checkpoint test conducted periodically
    /// (e.g. every N lessons) to track progress.
    /// </summary>
    Intermediate = 2,

    /// <summary>
    /// A formal internal examination administered before
    /// the student attempts the official ГИБДД test.
    /// </summary>
    InternalExam = 3,

    /// <summary>
    /// A simulation of the official ГИБДД theory examination,
    /// using the same rules and question pool.
    /// </summary>
    GibddSimulation = 4
}
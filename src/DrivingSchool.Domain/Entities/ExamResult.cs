using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Records the outcome of an individual student's participation
/// in a scheduled <see cref="ExamEvent"/>.
/// For theory exams the result is derived from a <see cref="TestSession"/>;
/// for practice exams it is entered manually by an instructor.
/// </summary>
public sealed class ExamResult : BaseEntity
{
    private ExamResult() { } // Required by EF Core

    /// <summary>Gets the identifier of the exam event this result belongs to.</summary>
    public Guid ExamEventId { get; private set; }

    /// <summary>Gets the student's contract identifier.</summary>
    public Guid ContractId { get; private set; }

    /// <summary>
    /// Gets the identifier of the theory test session linked to this result,
    /// or <c>null</c> for practice exam results.
    /// </summary>
    public Guid? SessionId { get; private set; }

    /// <summary>
    /// Gets the identifier of the instructor who evaluated the practice exam,
    /// or <c>null</c> for theory exam results.
    /// </summary>
    public Guid? InstructorId { get; private set; }

    /// <summary>Gets the number of correct answers or the score awarded.</summary>
    public int Score { get; private set; }

    /// <summary>Gets a value indicating whether the student passed.</summary>
    public bool IsPassed { get; private set; }

    /// <summary>Gets an optional evaluator's note about the student's performance.</summary>
    public string? Note { get; private set; }

    /// <summary>Creates a theory exam result derived from a completed test session.</summary>
    /// <param name="examEventId">The parent exam event identifier.</param>
    /// <param name="contractId">The student's contract identifier.</param>
    /// <param name="sessionId">The test session that produced this result.</param>
    /// <param name="score">The number of correct answers.</param>
    /// <param name="isPassed">Whether the student achieved the pass threshold.</param>
    public static ExamResult ForTheory(
        Guid examEventId,
        Guid contractId,
        Guid sessionId,
        int score,
        bool isPassed)
        => new()
        {
            ExamEventId = examEventId,
            ContractId = contractId,
            SessionId = sessionId,
            Score = score,
            IsPassed = isPassed
        };

    /// <summary>Creates a practice exam result entered manually by an instructor.</summary>
    /// <param name="examEventId">The parent exam event identifier.</param>
    /// <param name="contractId">The student's contract identifier.</param>
    /// <param name="instructorId">The evaluating instructor's identifier.</param>
    /// <param name="score">The score awarded by the instructor.</param>
    /// <param name="isPassed">Whether the student passed the practical assessment.</param>
    /// <param name="note">An optional evaluator's note.</param>
    public static ExamResult ForPractice(
        Guid examEventId,
        Guid contractId,
        Guid instructorId,
        int score,
        bool isPassed,
        string? note = null)
        => new()
        {
            ExamEventId = examEventId,
            ContractId = contractId,
            InstructorId = instructorId,
            Score = score,
            IsPassed = isPassed,
            Note = note
        };

    /// <summary>Corrects the result after it has been recorded.</summary>
    public void Update(int score, bool isPassed, string? note)
    {
        Score = score;
        IsPassed = isPassed;
        Note = note;
    }
}
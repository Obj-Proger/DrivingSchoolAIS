namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Records whether a student (identified by their contract) was present
/// at a specific theory lesson.
/// Attendance uses a composite key of <see cref="LessonId"/> and <see cref="ContractId"/>.
/// </summary>
public sealed class Attendance
{
    private Attendance() { } // Required by EF Core

    /// <summary>Gets the identifier of the lesson.</summary>
    public Guid LessonId { get; private set; }

    /// <summary>
    /// Gets the identifier of the student's contract.
    /// Attendance is tracked per contract, not per user account,
    /// to support students enrolled under multiple contracts.
    /// </summary>
    public Guid ContractId { get; private set; }

    /// <summary>Gets a value indicating whether the student was present at the lesson.</summary>
    public bool IsPresent { get; private set; }

    /// <summary>Gets an optional note from the teacher regarding this student's attendance.</summary>
    public string? Note { get; private set; }

    /// <summary>
    /// Creates a new attendance record.
    /// </summary>
    /// <param name="lessonId">The lesson identifier.</param>
    /// <param name="contractId">The student's contract identifier.</param>
    /// <param name="isPresent">Whether the student attended.</param>
    /// <param name="note">An optional teacher note.</param>
    public static Attendance Create(
        Guid lessonId,
        Guid contractId,
        bool isPresent,
        string? note = null)
        => new()
        {
            LessonId = lessonId,
            ContractId = contractId,
            IsPresent = isPresent,
            Note = note
        };

    /// <summary>
    /// Updates the attendance status and optional note.
    /// Called when a teacher corrects a previously recorded entry.
    /// </summary>
    /// <param name="isPresent">The corrected presence value.</param>
    /// <param name="note">The updated optional note.</param>
    public void Update(bool isPresent, string? note)
    {
        IsPresent = isPresent;
        Note = note;
    }
}
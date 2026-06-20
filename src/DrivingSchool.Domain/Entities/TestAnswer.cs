namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Records the answer submitted by a student for one question
/// within a <see cref="TestSession"/>.
/// Identified by the composite key (<see cref="SessionId"/>, <see cref="QuestionId"/>).
/// </summary>
public sealed class TestAnswer
{
    private TestAnswer() { } // Required by EF Core

    /// <summary>Gets the identifier of the parent session.</summary>
    public Guid SessionId { get; private set; }

    /// <summary>Gets the identifier of the question that was answered.</summary>
    public Guid QuestionId { get; private set; }

    /// <summary>
    /// Gets the identifier of the option chosen by the student,
    /// or <c>null</c> if the session timed out before this question was answered.
    /// </summary>
    public Guid? SelectedOptionId { get; private set; }

    /// <summary>Gets a value indicating whether the selected option is correct.</summary>
    public bool IsCorrect { get; private set; }

    /// <summary>Gets the UTC timestamp when the answer was submitted.</summary>
    public DateTime AnsweredAt { get; private set; }

    /// <summary>Creates a new answer record.</summary>
    /// <param name="sessionId">The parent session identifier.</param>
    /// <param name="questionId">The answered question identifier.</param>
    /// <param name="selectedOptionId">The chosen option, or <c>null</c> if unanswered.</param>
    /// <param name="isCorrect">Whether the answer is correct.</param>
    public static TestAnswer Create(
        Guid sessionId,
        Guid questionId,
        Guid? selectedOptionId,
        bool isCorrect)
        => new()
        {
            SessionId = sessionId,
            QuestionId = questionId,
            SelectedOptionId = selectedOptionId,
            IsCorrect = isCorrect,
            AnsweredAt = DateTime.UtcNow
        };
}
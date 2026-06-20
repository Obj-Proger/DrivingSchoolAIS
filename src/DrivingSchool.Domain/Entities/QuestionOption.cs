namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents one possible answer to a <see cref="Question"/>.
/// The correct option is identified via <see cref="Question.CorrectOptionId"/>.
/// </summary>
public sealed class QuestionOption
{
    private QuestionOption() { } // Required by EF Core

    /// <summary>Gets the unique identifier of this option.</summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>Gets the identifier of the question this option belongs to.</summary>
    public Guid QuestionId { get; private set; }

    /// <summary>Gets the display text of this answer option.</summary>
    public string Text { get; private set; } = string.Empty;

    /// <summary>
    /// Creates a new answer option for the specified question.
    /// </summary>
    /// <param name="questionId">The parent question identifier.</param>
    /// <param name="text">The option text.</param>
    public static QuestionOption Create(Guid questionId, string text)
        => new()
        {
            QuestionId = questionId,
            Text = text.Trim()
        };
}
using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a multiple-choice question in the test bank.
/// Each question belongs to a course topic and targets a specific licence category.
/// </summary>
public sealed class Question : BaseEntity
{
    private readonly List<QuestionOption> _options = [];

    private Question() { } // Required by EF Core

    /// <summary>Gets the identifier of the course topic this question relates to.</summary>
    public Guid TopicId { get; private set; }

    /// <summary>Gets the licence category this question is used to examine.</summary>
    public LicenseCategory Category { get; private set; }

    /// <summary>Gets the question text.</summary>
    public string Text { get; private set; } = string.Empty;

    /// <summary>Gets the URL of an optional illustrative image, or <c>null</c> if none.</summary>
    public string? ImageUrl { get; private set; }

    /// <summary>
    /// Gets an optional explanation of why the correct answer is correct.
    /// Shown to students after submitting an incorrect answer.
    /// </summary>
    public string? Explanation { get; private set; }

    /// <summary>Gets whether this question was taken from the official source or created by the school.</summary>
    public QuestionSource Source { get; private set; }

    /// <summary>
    /// Gets the identifier of the option that is the correct answer to this question.
    /// </summary>
    public Guid CorrectOptionId { get; private set; }

    /// <summary>Gets a value indicating whether this question is included in test sessions.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the list of answer options for this question.</summary>
    public IReadOnlyList<QuestionOption> Options => _options.AsReadOnly();

    // Factory

    /// <summary>
    /// Creates a new question with its answer options.
    /// </summary>
    /// <param name="topicId">The course topic identifier.</param>
    /// <param name="category">The target licence category.</param>
    /// <param name="text">The question text.</param>
    /// <param name="options">
    /// The answer options. At least two are required and exactly one must be marked correct.
    /// </param>
    /// <param name="correctOptionIndex">
    /// The zero-based index of the correct option in the <paramref name="options"/> list.
    /// </param>
    /// <param name="source">The origin of the question.</param>
    /// <param name="imageUrl">An optional image URL.</param>
    /// <param name="explanation">An optional explanation of the correct answer.</param>
    /// <returns>
    /// A successful <see cref="Result{Question}"/>,
    /// or a failure with <see cref="DomainErrors.Question.InsufficientOptions"/>
    /// or <see cref="DomainErrors.Question.InvalidCorrectOptionIndex"/>.
    /// </returns>
    public static Result<Question> Create(
        Guid topicId,
        LicenseCategory category,
        string text,
        IReadOnlyList<string> options,
        int correctOptionIndex,
        QuestionSource source,
        string? imageUrl = null,
        string? explanation = null)
    {
        if (options.Count < 2)
            return Result.Failure<Question>(DomainErrors.Question.InsufficientOptions);

        if (correctOptionIndex < 0 || correctOptionIndex >= options.Count)
            return Result.Failure<Question>(DomainErrors.Question.InvalidCorrectOptionIndex);

        var question = new Question
        {
            TopicId = topicId,
            Category = category,
            Text = text.Trim(),
            Source = source,
            ImageUrl = imageUrl,
            Explanation = explanation,
            IsActive = true
        };

        foreach (var optionText in options)
            question._options.Add(QuestionOption.Create(question.Id, optionText));

        question.CorrectOptionId = question._options[correctOptionIndex].Id;

        return Result.Success(question);
    }

    // Behaviour

    /// <summary>
    /// Determines whether the provided option identifier is the correct answer.
    /// </summary>
    /// <param name="selectedOptionId">The option chosen by the student.</param>
    public bool IsAnswerCorrect(Guid selectedOptionId) =>
        selectedOptionId == CorrectOptionId;

    /// <summary>Updates the question's content and correct answer.</summary>
    public void Update(string text, string? imageUrl, string? explanation, Guid correctOptionId)
    {
        Text = text.Trim();
        ImageUrl = imageUrl;
        Explanation = explanation;
        CorrectOptionId = correctOptionId;
    }

    /// <summary>Deactivates the question, excluding it from future test sessions.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Reactivates a previously deactivated question.</summary>
    public void Activate() => IsActive = true;
}
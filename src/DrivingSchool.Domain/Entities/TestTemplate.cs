using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Defines the configuration for a type of test session.
/// Templates are used to generate <see cref="TestSession"/> instances
/// with a consistent set of rules and question selection criteria.
/// </summary>
public sealed class TestTemplate : BaseEntity
{
    private TestTemplate() { } // Required by EF Core

    /// <summary>Gets the display name of this template (e.g. "Category B — Internal Exam").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the purpose of tests generated from this template.</summary>
    public TestType Type { get; private set; }

    /// <summary>
    /// Gets the identifier of the course this template belongs to,
    /// or <c>null</c> if it applies to all courses.
    /// </summary>
    public Guid? CourseId { get; private set; }

    /// <summary>
    /// Gets the list of topic identifiers from which questions are drawn.
    /// An empty list means questions are drawn from all topics in the course.
    /// </summary>
    public List<Guid> TopicIds { get; private set; } = [];

    /// <summary>
    /// Gets the licence category filter for question selection,
    /// or <c>null</c> to include all categories.
    /// </summary>
    public LicenseCategory? Category { get; private set; }

    /// <summary>Gets the total number of questions included in each session.</summary>
    public int QuestionCount { get; private set; }

    /// <summary>Gets the time limit for each session in minutes.</summary>
    public int TimeLimitMinutes { get; private set; }

    /// <summary>
    /// Gets the minimum number of correct answers required to pass.
    /// </summary>
    public int PassScore { get; private set; }

    /// <summary>Gets the strategy applied when the student gives an incorrect answer.</summary>
    public ErrorHandlingMode ErrorHandling { get; private set; }

    /// <summary>
    /// Gets the number of additional questions added per incorrect answer
    /// when <see cref="ErrorHandling"/> is <see cref="ErrorHandlingMode.AddQuestions"/>.
    /// </summary>
    public int AddQuestionsOnError { get; private set; }

    /// <summary>
    /// Gets the number of additional minutes added per incorrect answer
    /// when <see cref="ErrorHandling"/> is <see cref="ErrorHandlingMode.AddQuestions"/>.
    /// </summary>
    public int AddMinutesOnError { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this template is automatically assigned to students
    /// after a set number of lessons, as defined by <see cref="AutoAssignEveryNLessons"/>.
    /// </summary>
    public bool IsAutoAssigned { get; private set; }

    /// <summary>
    /// Gets the lesson interval at which the test is automatically assigned,
    /// or <c>null</c> when <see cref="IsAutoAssigned"/> is <c>false</c>.
    /// </summary>
    public int? AutoAssignEveryNLessons { get; private set; }

    /// <summary>Gets a value indicating whether this template is available for new sessions.</summary>
    public bool IsActive { get; private set; }

    // Factory

    /// <summary>Creates a new test template.</summary>
    /// <returns>
    /// A successful <see cref="Result{TestTemplate}"/>,
    /// or a failure describing which validation rule was violated.
    /// </returns>
    public static Result<TestTemplate> Create(
        string name,
        TestType type,
        int questionCount,
        int timeLimitMinutes,
        int passScore,
        ErrorHandlingMode errorHandling,
        int addQuestionsOnError = 0,
        int addMinutesOnError = 0,
        Guid? courseId = null,
        List<Guid>? topicIds = null,
        LicenseCategory? category = null,
        bool isAutoAssigned = false,
        int? autoAssignEveryNLessons = null)
    {
        if (questionCount <= 0)
            return Result.Failure<TestTemplate>(DomainErrors.TestTemplate.InvalidQuestionCount);

        if (timeLimitMinutes <= 0)
            return Result.Failure<TestTemplate>(DomainErrors.TestTemplate.InvalidTimeLimit);

        if (passScore <= 0 || passScore > questionCount)
            return Result.Failure<TestTemplate>(DomainErrors.TestTemplate.InvalidPassScore);

        if (isAutoAssigned && (autoAssignEveryNLessons is null or <= 0))
            return Result.Failure<TestTemplate>(DomainErrors.TestTemplate.InvalidAutoAssignInterval);

        return Result.Success(new TestTemplate
        {
            Name = name.Trim(),
            Type = type,
            QuestionCount = questionCount,
            TimeLimitMinutes = timeLimitMinutes,
            PassScore = passScore,
            ErrorHandling = errorHandling,
            AddQuestionsOnError = addQuestionsOnError,
            AddMinutesOnError = addMinutesOnError,
            CourseId = courseId,
            TopicIds = topicIds ?? [],
            Category = category,
            IsAutoAssigned = isAutoAssigned,
            AutoAssignEveryNLessons = autoAssignEveryNLessons,
            IsActive = true
        });
    }

    /// <summary>Updates the template's configuration.</summary>
    public void Update(
        string name,
        int questionCount,
        int timeLimitMinutes,
        int passScore,
        ErrorHandlingMode errorHandling,
        int addQuestionsOnError,
        int addMinutesOnError)
    {
        Name = name.Trim();
        QuestionCount = questionCount;
        TimeLimitMinutes = timeLimitMinutes;
        PassScore = passScore;
        ErrorHandling = errorHandling;
        AddQuestionsOnError = addQuestionsOnError;
        AddMinutesOnError = addMinutesOnError;
    }

    /// <summary>Deactivates the template.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Reactivates the template.</summary>
    public void Activate() => IsActive = true;
}
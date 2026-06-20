using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a single attempt by a student to complete a test defined by a <see cref="TestTemplate"/>.
/// The session tracks the assigned questions, student answers, timing, and final result.
/// </summary>
public sealed class TestSession : BaseEntity
{
    private readonly List<TestAnswer> _answers = [];
    private readonly List<Guid> _questionIds = [];

    private TestSession() { } // Required by EF Core

    /// <summary>Gets the contract identifier of the student taking this test.</summary>
    public Guid ContractId { get; private set; }

    /// <summary>Gets the template that defines the rules for this session.</summary>
    public Guid TemplateId { get; private set; }

    /// <summary>Gets the UTC timestamp when the session started.</summary>
    public DateTime StartedAt { get; private set; }

    /// <summary>Gets the UTC timestamp when the session was submitted or timed out.</summary>
    public DateTime? FinishedAt { get; private set; }

    /// <summary>Gets the UTC deadline by which the student must finish.</summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>Gets the current status of the session.</summary>
    public SessionStatus Status { get; private set; }

    /// <summary>Gets the number of correct answers at completion.</summary>
    public int Score { get; private set; }

    /// <summary>Gets the total number of questions in this session (including bonus questions).</summary>
    public int TotalQuestions { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the student passed,
    /// or <c>null</c> while the session is still in progress.
    /// </summary>
    public bool? IsPassed { get; private set; }

    /// <summary>Gets the number of bonus questions added due to error handling, or <c>null</c>.</summary>
    public int? BonusQuestions { get; private set; }

    /// <summary>Gets the number of bonus minutes added due to error handling, or <c>null</c>.</summary>
    public int? BonusMinutes { get; private set; }

    /// <summary>Gets the identifiers of the questions assigned to this session, in order.</summary>
    public IReadOnlyList<Guid> QuestionIds => _questionIds.AsReadOnly();

    /// <summary>Gets the answers submitted so far.</summary>
    public IReadOnlyList<TestAnswer> Answers => _answers.AsReadOnly();

    /// <summary>
    /// Gets a value indicating whether the session time limit has elapsed.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    // Factory

    /// <summary>
    /// Starts a new test session with a pre-selected set of questions.
    /// </summary>
    /// <param name="contractId">The student's contract identifier.</param>
    /// <param name="templateId">The template governing the session rules.</param>
    /// <param name="questionIds">The ordered list of question identifiers selected for this session.</param>
    /// <param name="timeLimitMinutes">The initial time limit in minutes.</param>
    public static TestSession Start(
        Guid contractId,
        Guid templateId,
        IEnumerable<Guid> questionIds,
        int timeLimitMinutes)
    {
        var session = new TestSession
        {
            ContractId = contractId,
            TemplateId = templateId,
            StartedAt = DateTime.UtcNow,
            Status = SessionStatus.InProgress
        };

        session._questionIds.AddRange(questionIds);
        session.TotalQuestions = session._questionIds.Count;
        session.ExpiresAt = session.StartedAt.AddMinutes(timeLimitMinutes);

        return session;
    }

    // Behaviour

    /// <summary>
    /// Records the student's answer to a question.
    /// Automatically times out the session if the time limit has elapsed.
    /// </summary>
    /// <param name="questionId">The question being answered.</param>
    /// <param name="selectedOptionId">The chosen option identifier.</param>
    /// <param name="isCorrect">Whether the selected option is the correct answer.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TestSession.NotInProgress"/>,
    /// <see cref="DomainErrors.TestSession.TimedOut"/>,
    /// or <see cref="DomainErrors.TestSession.AlreadyAnswered"/>.
    /// </returns>
    public Result SubmitAnswer(Guid questionId, Guid selectedOptionId, bool isCorrect)
    {
        if (Status != SessionStatus.InProgress)
            return Result.Failure(DomainErrors.TestSession.NotInProgress);

        if (IsExpired)
        {
            TimeOut();
            return Result.Failure(DomainErrors.TestSession.TimedOut);
        }

        if (_answers.Any(a => a.QuestionId == questionId))
            return Result.Failure(DomainErrors.TestSession.AlreadyAnswered);

        _answers.Add(TestAnswer.Create(Id, questionId, selectedOptionId, isCorrect));
        return Result.Success();
    }

    /// <summary>
    /// Finalises the session and calculates the result.
    /// </summary>
    /// <param name="passScore">
    /// The minimum number of correct answers required to pass,
    /// taken from the parent <see cref="TestTemplate"/>.
    /// </param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.TestSession.NotInProgress"/>.
    /// </returns>
    public Result Finish(int passScore)
    {
        if (Status != SessionStatus.InProgress)
            return Result.Failure(DomainErrors.TestSession.NotInProgress);

        Status = SessionStatus.Completed;
        FinishedAt = DateTime.UtcNow;
        Score = _answers.Count(a => a.IsCorrect);
        IsPassed = Score >= passScore;

        return Result.Success();
    }

    /// <summary>
    /// Times out the session when the time limit elapses.
    /// Called automatically by <see cref="SubmitAnswer"/> or explicitly by a background job.
    /// </summary>
    public void TimeOut()
    {
        if (Status != SessionStatus.InProgress) return;

        Status = SessionStatus.TimedOut;
        FinishedAt = DateTime.UtcNow;
        Score = _answers.Count(a => a.IsCorrect);
        IsPassed = false;
    }

    /// <summary>
    /// Adds bonus questions and time to the session.
    /// Called by the Application layer when <see cref="ErrorHandlingMode.AddQuestions"/> is active.
    /// </summary>
    /// <param name="additionalQuestionIds">The identifiers of the extra questions to append.</param>
    /// <param name="additionalMinutes">The number of extra minutes to add to the deadline.</param>
    public void AddBonusResources(IEnumerable<Guid> additionalQuestionIds, int additionalMinutes)
    {
        var extraIds = additionalQuestionIds.ToList();

        _questionIds.AddRange(extraIds);
        TotalQuestions += extraIds.Count;
        BonusQuestions = (BonusQuestions ?? 0) + extraIds.Count;

        if (additionalMinutes > 0)
        {
            ExpiresAt = ExpiresAt.AddMinutes(additionalMinutes);
            BonusMinutes = (BonusMinutes ?? 0) + additionalMinutes;
        }
    }
}
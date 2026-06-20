using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a scheduled examination for a training group.
/// Manages the exam results of all students participating in this event.
/// </summary>
public sealed class ExamEvent : BaseEntity
{
    private readonly List<ExamResult> _results = [];

    private ExamEvent() { } // Required by EF Core

    /// <summary>Gets the identifier of the group sitting this exam.</summary>
    public Guid GroupId { get; private set; }

    /// <summary>
    /// Gets the identifier of the test template used for the theory exam,
    /// or <c>null</c> for practice exams.
    /// </summary>
    public Guid? TemplateId { get; private set; }

    /// <summary>Gets the UTC timestamp when the exam is scheduled to take place.</summary>
    public DateTime ScheduledAt { get; private set; }

    /// <summary>Gets whether this is a theory or practical exam.</summary>
    public ExamType Type { get; private set; }

    /// <summary>Gets the results of all students who have completed this exam.</summary>
    public IReadOnlyList<ExamResult> Results => _results.AsReadOnly();

    // Factory

    /// <summary>Creates a new exam event for a training group.</summary>
    /// <param name="groupId">The target group identifier.</param>
    /// <param name="scheduledAt">The UTC date and time of the exam.</param>
    /// <param name="type">Theory or practical.</param>
    /// <param name="templateId">
    /// The test template identifier (required for theory exams, optional for practice).
    /// </param>
    /// <returns>
    /// A successful <see cref="Result{ExamEvent}"/>,
    /// or a failure with <see cref="DomainErrors.ExamEvent.TheoryRequiresTemplate"/>.
    /// </returns>
    public static Result<ExamEvent> Create(
        Guid groupId,
        DateTime scheduledAt,
        ExamType type,
        Guid? templateId = null)
    {
        if (type == ExamType.Theory && templateId is null)
            return Result.Failure<ExamEvent>(DomainErrors.ExamEvent.TheoryRequiresTemplate);

        return Result.Success(new ExamEvent
        {
            GroupId = groupId,
            ScheduledAt = scheduledAt,
            Type = type,
            TemplateId = templateId
        });
    }

    // Behaviour

    /// <summary>
    /// Records a student's result for this exam.
    /// If a result for the same contract already exists it is replaced.
    /// </summary>
    /// <param name="result">The exam result to record.</param>
    public void RecordResult(ExamResult result)
    {
        var existing = _results.FirstOrDefault(r => r.ContractId == result.ContractId);
        if (existing is not null)
            _results.Remove(existing);

        _results.Add(result);
    }

    /// <summary>Reschedules the exam to a new date and time.</summary>
    /// <param name="newScheduledAt">The new UTC date and time.</param>
    public void Reschedule(DateTime newScheduledAt) => ScheduledAt = newScheduledAt;
}
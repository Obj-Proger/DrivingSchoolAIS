namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="ExamEvent"/> aggregate.
/// </summary>
public interface IExamEventRepository
{
    /// <summary>Returns the exam event with the specified identifier, or <c>null</c> if not found.</summary>
    Task<ExamEvent?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all exam events for the specified group,
    /// ordered by scheduled date ascending.
    /// </summary>
    Task<IReadOnlyList<ExamEvent>> GetByGroupIdAsync(
        Guid groupId,
        CancellationToken ct = default);

    /// <summary>
    /// Returns all exam results for the specified contract across all exams,
    /// ordered by scheduled date descending.
    /// </summary>
    Task<IReadOnlyList<ExamResult>> GetResultsByContractAsync(
        Guid contractId,
        CancellationToken ct = default);

    /// <summary>Adds a new exam event to the repository.</summary>
    Task AddAsync(ExamEvent examEvent, CancellationToken ct = default);

    /// <summary>Marks an existing exam event as modified.</summary>
    void Update(ExamEvent examEvent);
}
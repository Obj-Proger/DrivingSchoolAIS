namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="TestSession"/> aggregate.
/// </summary>
public interface ITestSessionRepository
{
    /// <summary>Returns the session with the specified identifier, or <c>null</c> if not found.</summary>
    Task<TestSession?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the active (in-progress) session for the specified contract,
    /// or <c>null</c> if no session is currently in progress.
    /// </summary>
    Task<TestSession?> GetActiveByContractAsync(Guid contractId, CancellationToken ct = default);

    /// <summary>
    /// Returns all completed and timed-out sessions for the specified contract,
    /// ordered by start time descending.
    /// </summary>
    Task<IReadOnlyList<TestSession>> GetHistoryByContractAsync(
        Guid contractId,
        CancellationToken ct = default);

    /// <summary>
    /// Returns the number of theory lessons attended since the last auto-assigned
    /// test session for this contract. Used to determine when the next auto-test is due.
    /// </summary>
    Task<int> GetLessonsAttendedSinceLastAutoTestAsync(
        Guid contractId,
        CancellationToken ct = default);

    /// <summary>Adds a new test session to the repository.</summary>
    Task AddAsync(TestSession session, CancellationToken ct = default);

    /// <summary>Marks an existing test session as modified.</summary>
    void Update(TestSession session);
}
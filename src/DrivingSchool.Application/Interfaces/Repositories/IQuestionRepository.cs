using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Question"/> aggregate.
/// </summary>
public interface IQuestionRepository
{
    /// <summary>Returns the question with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Question?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns a paginated, optionally filtered list of questions.</summary>
    Task<PaginatedResult<Question>> GetPaginatedAsync(
        int page,
        int pageSize,
        Guid? topicId = null,
        LicenseCategory? category = null,
        QuestionSource? source = null,
        string? search = null,
        CancellationToken ct = default);

    /// <summary>
    /// Returns a random selection of active questions matching the specified filters,
    /// used to populate a <see cref="TestSession"/>.
    /// </summary>
    Task<IReadOnlyList<Question>> GetRandomAsync(
        int count,
        IEnumerable<Guid>? topicIds = null,
        LicenseCategory? category = null,
        CancellationToken ct = default);

    /// <summary>Adds a new question to the repository.</summary>
    Task AddAsync(Question question, CancellationToken ct = default);

    /// <summary>Adds multiple questions in a single operation. Used for bulk import.</summary>
    Task AddRangeAsync(IEnumerable<Question> questions, CancellationToken ct = default);

    /// <summary>Marks an existing question as modified.</summary>
    void Update(Question question);
}
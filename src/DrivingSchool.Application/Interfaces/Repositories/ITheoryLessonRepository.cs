namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="TheoryLesson"/> aggregate.
/// </summary>
public interface ITheoryLessonRepository
{
    /// <summary>Returns the lesson with the specified identifier, or <c>null</c> if not found.</summary>
    Task<TheoryLesson?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the lesson with its materials and attendance records eagerly loaded,
    /// or <c>null</c> if not found.
    /// </summary>
    Task<TheoryLesson?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all lessons for the specified group within an optional date range,
    /// ordered by <see cref="TheoryLesson.ScheduledAt"/> ascending.
    /// </summary>
    Task<IReadOnlyList<TheoryLesson>> GetByGroupIdAsync(
        Guid groupId,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default);

    /// <summary>Adds a new theory lesson to the repository.</summary>
    Task AddAsync(TheoryLesson lesson, CancellationToken ct = default);

    /// <summary>Marks an existing theory lesson as modified.</summary>
    void Update(TheoryLesson lesson);

    /// <summary>Removes a theory lesson from the repository.</summary>
    void Delete(TheoryLesson lesson);
}
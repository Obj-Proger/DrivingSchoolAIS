namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Course"/> aggregate.
/// </summary>
public interface ICourseRepository
{
    /// <summary>Returns the course with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Course?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the course with its topics collection eagerly loaded,
    /// or <c>null</c> if not found.
    /// </summary>
    Task<Course?> GetByIdWithTopicsAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all courses, optionally restricted to active ones only.
    /// </summary>
    /// <param name="activeOnly">When <c>true</c>, excludes deactivated courses.</param>
    Task<IReadOnlyList<Course>> GetAllAsync(bool activeOnly = false, CancellationToken ct = default);

    /// <summary>Adds a new course to the repository.</summary>
    Task AddAsync(Course course, CancellationToken ct = default);

    /// <summary>Marks an existing course as modified.</summary>
    void Update(Course course);
}
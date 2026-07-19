namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="TestTemplate"/> aggregate.
/// </summary>
public interface ITestTemplateRepository
{
    /// <summary>Returns the template with the specified identifier, or <c>null</c> if not found.</summary>
    Task<TestTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all templates, optionally restricted to active ones only
    /// and/or filtered by course.
    /// </summary>
    /// <param name="activeOnly">When <c>true</c>, excludes deactivated templates.</param>
    /// <param name="courseId">When provided, restricts results to this course (or global templates).</param>
    Task<IReadOnlyList<TestTemplate>> GetAllAsync(
        bool activeOnly = false,
        Guid? courseId = null,
        CancellationToken ct = default);

    /// <summary>Adds a new test template to the repository.</summary>
    Task AddAsync(TestTemplate template, CancellationToken ct = default);

    /// <summary>Marks an existing test template as modified.</summary>
    void Update(TestTemplate template);
}
namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Branch"/> aggregate.
/// </summary>
public interface IBranchRepository
{
    /// <summary>Returns the branch with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all branches, optionally restricted to active ones only.
    /// Used to populate branch selection dropdowns and for report grouping.
    /// </summary>
    /// <param name="activeOnly">When <c>true</c>, excludes deactivated branches.</param>
    Task<IReadOnlyList<Branch>> GetAllAsync(bool activeOnly = false, CancellationToken ct = default);

    /// <summary>Adds a new branch to the repository.</summary>
    Task AddAsync(Branch branch, CancellationToken ct = default);

    /// <summary>Marks an existing branch as modified.</summary>
    void Update(Branch branch);
}
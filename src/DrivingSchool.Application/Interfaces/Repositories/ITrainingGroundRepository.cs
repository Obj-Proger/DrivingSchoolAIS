namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="TrainingGround"/> aggregate.
/// </summary>
public interface ITrainingGroundRepository
{
    /// <summary>Returns the training ground with the specified identifier, or <c>null</c> if not found.</summary>
    Task<TrainingGround?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all training grounds, optionally restricted to active ones only.
    /// </summary>
    /// <param name="activeOnly">When <c>true</c>, excludes deactivated grounds.</param>
    Task<IReadOnlyList<TrainingGround>> GetAllAsync(bool activeOnly = false, CancellationToken ct = default);

    /// <summary>Adds a new training ground to the repository.</summary>
    Task AddAsync(TrainingGround ground, CancellationToken ct = default);

    /// <summary>Marks an existing training ground as modified.</summary>
    void Update(TrainingGround ground);
}
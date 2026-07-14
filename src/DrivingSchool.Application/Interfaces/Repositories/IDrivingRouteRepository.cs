namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="DrivingRoute"/> aggregate.
/// </summary>
public interface IDrivingRouteRepository
{
    /// <summary>Returns the driving route with the specified identifier, or <c>null</c> if not found.</summary>
    Task<DrivingRoute?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns all driving routes created by the specified instructor.</summary>
    Task<IReadOnlyList<DrivingRoute>> GetByInstructorIdAsync(Guid instructorId, CancellationToken ct = default);

    /// <summary>Adds a new driving route to the repository.</summary>
    Task AddAsync(DrivingRoute route, CancellationToken ct = default);

    /// <summary>Marks an existing driving route as modified.</summary>
    void Update(DrivingRoute route);

    /// <summary>Removes a driving route from the repository.</summary>
    void Delete(DrivingRoute route);
}
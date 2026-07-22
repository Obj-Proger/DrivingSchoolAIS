namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Vehicle"/> aggregate.
/// </summary>
public interface IVehicleRepository
{
    /// <summary>Returns the vehicle with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all vehicles, optionally restricted to active ones only.
    /// </summary>
    /// <param name="activeOnly">When <c>true</c>, excludes deactivated vehicles.</param>
    Task<IReadOnlyList<Vehicle>> GetAllAsync(bool activeOnly = false, CancellationToken ct = default);

    /// <summary>
    /// Returns <c>true</c> if the specified plate number is already registered
    /// to another vehicle.
    /// </summary>
    Task<bool> ExistsByPlateNumberAsync(
        string plateNumber,
        Guid? excludeVehicleId = null,
        CancellationToken ct = default);

    /// <summary>Adds a new vehicle to the repository.</summary>
    Task AddAsync(Vehicle vehicle, CancellationToken ct = default);

    /// <summary>Marks an existing vehicle as modified.</summary>
    void Update(Vehicle vehicle);
}
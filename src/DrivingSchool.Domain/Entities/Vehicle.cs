using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a training vehicle owned by the driving school.
/// </summary>
public sealed class Vehicle : BaseEntity
{
    private Vehicle() { } // Required by EF Core

    /// <summary>Gets the official registration plate number.</summary>
    public string PlateNumber { get; private set; } = string.Empty;

    /// <summary>Gets the vehicle model name (e.g. "Toyota Corolla").</summary>
    public string Model { get; private set; } = string.Empty;

    /// <summary>Gets the year of manufacture.</summary>
    public int Year { get; private set; }

    /// <summary>Gets the licence category this vehicle is used to train.</summary>
    public LicenseCategory Category { get; private set; }

    /// <summary>Gets a value indicating whether this vehicle is available for scheduling.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Creates a new active training vehicle.</summary>
    /// <param name="plateNumber">The registration plate number.</param>
    /// <param name="model">The vehicle model name.</param>
    /// <param name="year">The year of manufacture.</param>
    /// <param name="category">The licence category this vehicle is used to train.</param>
    public static Vehicle Create(
        string plateNumber,
        string model,
        int year,
        LicenseCategory category)
        => new()
        {
            PlateNumber = plateNumber.Trim().ToUpperInvariant(),
            Model = model.Trim(),
            Year = year,
            Category = category,
            IsActive = true
        };

    /// <summary>Updates the vehicle's registration and model information.</summary>
    public void Update(string plateNumber, string model, int year)
    {
        PlateNumber = plateNumber.Trim().ToUpperInvariant();
        Model = model.Trim();
        Year = year;
    }

    /// <summary>Deactivates the vehicle, removing it from scheduling.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Reactivates a previously deactivated vehicle.</summary>
    public void Activate() => IsActive = true;
}
using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a training ground (practice area) available for practical lessons.
/// </summary>
public sealed class TrainingGround : BaseEntity
{
    private TrainingGround() { } // Required by EF Core

    /// <summary>Gets the name of the training ground.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the physical address of the training ground.</summary>
    public string Address { get; private set; } = string.Empty;

    /// <summary>Gets an optional description of the available facilities.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets a value indicating whether this ground is available for scheduling.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Creates a new active training ground.</summary>
    public static TrainingGround Create(string name, string address, string? description = null)
        => new()
        {
            Name = name.Trim(),
            Address = address.Trim(),
            Description = description,
            IsActive = true
        };

    /// <summary>Updates the ground's information.</summary>
    public void Update(string name, string address, string? description)
    {
        Name = name.Trim();
        Address = address.Trim();
        Description = description;
    }

    /// <summary>Deactivates the training ground.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Reactivates a previously deactivated training ground.</summary>
    public void Activate() => IsActive = true;
}
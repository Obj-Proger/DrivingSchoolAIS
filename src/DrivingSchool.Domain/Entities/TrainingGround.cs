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

    /// <summary>
    /// Gets the identifier of the branch this training ground belongs to,
    /// or <c>null</c> if not assigned to a specific branch.
    /// </summary>
    public Guid? BranchId { get; private set; }

    /// <summary>Creates a new active training ground.</summary>
    /// <param name="name">The name of the training ground.</param>
    /// <param name="address">The physical address.</param>
    /// <param name="description">An optional description of the available facilities.</param>
    /// <param name="branchId">The branch this ground belongs to (optional).</param>
    public static TrainingGround Create(
        string name,
        string address,
        string? description = null,
        Guid? branchId = null)
        => new()
        {
            Name = name.Trim(),
            Address = address.Trim(),
            Description = description,
            IsActive = true,
            BranchId = branchId
        };

    /// <summary>Updates the ground's information.</summary>
    public void Update(string name, string address, string? description)
    {
        Name = name.Trim();
        Address = address.Trim();
        Description = description;
    }

    /// <summary>Assigns the training ground to a branch.</summary>
    /// <param name="branchId">The identifier of the branch.</param>
    public void AssignBranch(Guid branchId) => BranchId = branchId;

    /// <summary>Deactivates the training ground.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Reactivates a previously deactivated training ground.</summary>
    public void Activate() => IsActive = true;
}
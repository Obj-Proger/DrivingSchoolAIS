using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a physical office (location) of the driving school.
/// Used by schools operating multiple sites to scope leads, contracts,
/// groups, training grounds, and staff to a specific location,
/// and to break down financial and operational reports per site.
/// </summary>
public sealed class Branch : BaseEntity
{
    private Branch() { } // Required by EF Core

    /// <summary>Gets the display name of the branch (e.g. "Центральный офис").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the city the branch is located in.</summary>
    public string City { get; private set; } = string.Empty;

    /// <summary>Gets the physical address of the branch.</summary>
    public string Address { get; private set; } = string.Empty;

    /// <summary>Gets the branch's contact phone number.</summary>
    public PhoneNumber Phone { get; private set; } = null!;

    /// <summary>Gets a value indicating whether the branch is currently operating.</summary>
    public bool IsActive { get; private set; }

    // Factory

    /// <summary>
    /// Creates a new active branch.
    /// </summary>
    /// <param name="name">The display name of the branch.</param>
    /// <param name="city">The city the branch is located in.</param>
    /// <param name="address">The physical address.</param>
    /// <param name="phone">The branch's contact phone number.</param>
    /// <returns>
    /// A successful <see cref="Result{Branch}"/>,
    /// or a failure with <see cref="DomainErrors.Branch.NameEmpty"/>
    /// or <see cref="DomainErrors.Branch.AddressEmpty"/>.
    /// </returns>
    public static Result<Branch> Create(
        string name,
        string city,
        string address,
        PhoneNumber phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Branch>(DomainErrors.Branch.NameEmpty);

        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure<Branch>(DomainErrors.Branch.AddressEmpty);

        return Result.Success(new Branch
        {
            Name = name.Trim(),
            City = city.Trim(),
            Address = address.Trim(),
            Phone = phone,
            IsActive = true
        });
    }

    // Behaviour

    /// <summary>Updates the branch's display information.</summary>
    /// <param name="name">The updated display name.</param>
    /// <param name="city">The updated city.</param>
    /// <param name="address">The updated physical address.</param>
    /// <param name="phone">The updated contact phone number.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Branch.NameEmpty"/>
    /// or <see cref="DomainErrors.Branch.AddressEmpty"/>.
    /// </returns>
    public Result Update(string name, string city, string address, PhoneNumber phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(DomainErrors.Branch.NameEmpty);

        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure(DomainErrors.Branch.AddressEmpty);

        Name = name.Trim();
        City = city.Trim();
        Address = address.Trim();
        Phone = phone;

        return Result.Success();
    }

    /// <summary>
    /// Deactivates the branch, hiding it from selection when creating
    /// new leads, contracts, groups, or staff assignments.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Branch.AlreadyInactive"/>.
    /// </returns>
    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure(DomainErrors.Branch.AlreadyInactive);

        IsActive = false;
        return Result.Success();
    }

    /// <summary>Reactivates a previously deactivated branch.</summary>
    public void Activate() => IsActive = true;
}
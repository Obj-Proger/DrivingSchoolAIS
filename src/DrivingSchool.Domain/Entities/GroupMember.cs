namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents the membership of a student (via their contract) in a training group.
/// Membership is tracked by contract rather than by user,
/// allowing a student to be enrolled in different groups under separate contracts.
/// </summary>
public sealed class GroupMember
{
    private GroupMember() { } // Required by EF Core

    /// <summary>Gets the identifier of the group.</summary>
    public Guid GroupId { get; private set; }

    /// <summary>
    /// Gets the identifier of the contract under which the student is enrolled.
    /// A student may hold multiple contracts (e.g. for different licence categories).
    /// </summary>
    public Guid ContractId { get; private set; }

    /// <summary>Gets the UTC timestamp when the student joined the group.</summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>
    /// Creates a new group membership record.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="contractId">The contract identifier of the enrolling student.</param>
    public static GroupMember Create(Guid groupId, Guid contractId)
        => new()
        {
            GroupId = groupId,
            ContractId = contractId,
            JoinedAt = DateTime.UtcNow
        };
}
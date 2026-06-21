using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Group"/> aggregate.
/// </summary>
public interface IGroupRepository
{
    /// <summary>Returns the group with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Group?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the group with its members collection eagerly loaded,
    /// or <c>null</c> if not found.
    /// </summary>
    Task<Group?> GetByIdWithMembersAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns a paginated, optionally filtered list of groups.</summary>
    Task<PaginatedResult<Group>> GetPaginatedAsync(
        int page,
        int pageSize,
        GroupStatus? status = null,
        Guid? teacherId = null,
        CancellationToken ct = default);

    /// <summary>Returns all groups assigned to the specified teacher.</summary>
    Task<IReadOnlyList<Group>> GetByTeacherIdAsync(Guid teacherId, CancellationToken ct = default);

    /// <summary>
    /// Returns <c>true</c> if the specified contract is currently enrolled
    /// in the specified group.
    /// </summary>
    Task<bool> IsContractMemberAsync(Guid contractId, Guid groupId, CancellationToken ct = default);

    /// <summary>Adds a new group to the repository.</summary>
    Task AddAsync(Group group, CancellationToken ct = default);

    /// <summary>Marks an existing group as modified.</summary>
    void Update(Group group);
}
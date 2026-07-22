using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="StaffTask"/> aggregate.
/// </summary>
public interface IStaffTaskRepository
{
    /// <summary>Returns the task with the specified identifier, or <c>null</c> if not found.</summary>
    Task<StaffTask?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns a paginated list of tasks assigned to the specified user.</summary>
    Task<PaginatedResult<StaffTask>> GetByAssigneeAsync(
        Guid assignedToId,
        int page,
        int pageSize,
        TaskItemStatus? status = null,
        CancellationToken ct = default);

    /// <summary>Returns a paginated list of all tasks, visible to managers and admins.</summary>
    Task<PaginatedResult<StaffTask>> GetAllAsync(
        int page,
        int pageSize,
        Guid? assignedToId = null,
        TaskItemStatus? status = null,
        CancellationToken ct = default);

    /// <summary>Adds a new task to the repository.</summary>
    Task AddAsync(StaffTask task, CancellationToken ct = default);

    /// <summary>Marks an existing task as modified.</summary>
    void Update(StaffTask task);

    /// <summary>Removes a task from the repository.</summary>
    void Delete(StaffTask task);

    /// <summary>
    /// Returns per-assignee task activity statistics for tasks created within
    /// the specified date range. A task counts as overdue when it is not yet
    /// <see cref="TaskItemStatus.Done"/> and its due date has passed.
    /// </summary>
    Task<IReadOnlyList<StaffActivityStats>> GetActivityStatsAsync(
        DateTime from,
        DateTime to,
        CancellationToken ct = default);
}

/// <summary>Aggregate task activity statistics for a single staff member.</summary>
public sealed record StaffActivityStats(
    Guid AssignedToId,
    int TotalCount,
    int CompletedCount,
    int OverdueCount);
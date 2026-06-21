using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Tasks.DTOs;

namespace DrivingSchool.Application.Features.Tasks.Queries.GetAllTasks;

/// <summary>
/// Returns a paginated list of all tasks. Accessible to managers and administrators.
/// </summary>
public sealed record GetAllTasksQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? AssignedToId = null,
    TaskItemStatus? Status = null) : IQuery<PaginatedResult<StaffTaskDto>>;
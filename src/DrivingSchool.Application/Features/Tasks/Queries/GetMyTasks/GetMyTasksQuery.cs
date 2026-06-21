using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Tasks.DTOs;

namespace DrivingSchool.Application.Features.Tasks.Queries.GetMyTasks;

/// <summary>Returns a paginated list of tasks assigned to the current user.</summary>
public sealed record GetMyTasksQuery(
    int Page = 1,
    int PageSize = 20,
    TaskItemStatus? Status = null) : IQuery<PaginatedResult<StaffTaskDto>>;
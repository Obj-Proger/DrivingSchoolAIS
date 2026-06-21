using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Groups.DTOs;

namespace DrivingSchool.Application.Features.Groups.Queries.GetGroups;

/// <summary>
/// Returns a paginated list of groups with optional filters.
/// </summary>
public sealed record GetGroupsQuery(
    int Page = 1,
    int PageSize = 20,
    GroupStatus? Status = null,
    Guid? TeacherId = null) : IQuery<PaginatedResult<GroupDto>>;
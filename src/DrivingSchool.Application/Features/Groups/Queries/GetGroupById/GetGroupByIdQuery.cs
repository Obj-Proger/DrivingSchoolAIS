using DrivingSchool.Application.Features.Groups.DTOs;

namespace DrivingSchool.Application.Features.Groups.Queries.GetGroupById;

/// <summary>
/// Returns the full detail view of a group including its member list.
/// </summary>
public sealed record GetGroupByIdQuery(Guid GroupId) : IQuery<GroupDetailDto>;
using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetDrivingRoutes;

/// <summary>
/// Returns all driving routes created by the specified instructor.
/// </summary>
public sealed record GetDrivingRoutesQuery(Guid InstructorId)
    : IQuery<IReadOnlyList<DrivingRouteDto>>;
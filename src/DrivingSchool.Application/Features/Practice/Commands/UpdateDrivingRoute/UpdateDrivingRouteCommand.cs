using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.UpdateDrivingRoute;

/// <summary>
/// Updates a driving route's display information.
/// An instructor may only update their own routes.
/// </summary>
public sealed record UpdateDrivingRouteCommand(
    Guid RouteId,
    string Name,
    string Description,
    string? MapData) : ICommand;
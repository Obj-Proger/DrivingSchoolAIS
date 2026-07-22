using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.DeleteDrivingRoute;

/// <summary>
/// Deletes a driving route. An instructor may only delete their own routes.
/// </summary>
public sealed record DeleteDrivingRouteCommand(Guid RouteId) : ICommand;
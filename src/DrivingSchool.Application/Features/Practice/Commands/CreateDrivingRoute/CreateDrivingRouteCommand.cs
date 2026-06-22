using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CreateDrivingRoute;

/// <summary>Creates a new driving route for use in practice lessons.</summary>
public sealed record CreateDrivingRouteCommand(
    string Name,
    string Description,
    string? MapData = null) : ICommand<Guid>;
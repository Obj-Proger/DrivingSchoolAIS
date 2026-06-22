using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CreateTrainingGround;

/// <summary>Creates a new training ground available for practice bookings.</summary>
public sealed record CreateTrainingGroundCommand(
    string Name,
    string Address,
    string? Description = null) : ICommand<Guid>;
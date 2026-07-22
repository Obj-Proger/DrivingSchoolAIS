using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.UpdateTrainingGround;

/// <summary>Updates a training ground's display information.</summary>
public sealed record UpdateTrainingGroundCommand(
    Guid GroundId,
    string Name,
    string Address,
    string? Description) : ICommand;
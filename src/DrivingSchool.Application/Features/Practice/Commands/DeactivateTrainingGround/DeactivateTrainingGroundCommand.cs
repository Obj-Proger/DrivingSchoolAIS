using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.DeactivateTrainingGround;

/// <summary>Deactivates a training ground, hiding it from slot scheduling.</summary>
public sealed record DeactivateTrainingGroundCommand(Guid GroundId) : ICommand;
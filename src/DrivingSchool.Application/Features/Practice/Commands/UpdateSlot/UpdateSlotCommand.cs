using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.UpdateSlot;

/// <summary>Updates the scheduling and resource assignment of an Available slot.</summary>
public sealed record UpdateSlotCommand(
    Guid SlotId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    Guid? VehicleId,
    Guid? DefaultTrainingGroundId,
    string? Note) : ICommand;
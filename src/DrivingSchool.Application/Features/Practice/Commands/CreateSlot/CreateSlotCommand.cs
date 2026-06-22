using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CreateSlot;

/// <summary>
/// Creates a single practice slot for the current instructor.
/// </summary>
public sealed record CreateSlotCommand(
    DateTime StartDateTime,
    DateTime EndDateTime,
    SlotType Type,
    Guid? VehicleId = null,
    Guid? DefaultTrainingGroundId = null,
    bool IsOpenForStudentGroundChoice = false,
    string? Note = null) : ICommand<Guid>;
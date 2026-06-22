using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.BookSlot;

/// <summary>
/// Books a practice slot for the specified student contract.
/// The student may optionally select a training ground
/// if the slot is open for student ground choice.
/// Returns the identifier of the created booking.
/// </summary>
public sealed record BookSlotCommand(
    Guid SlotId,
    Guid ContractId,
    Guid? SelectedTrainingGroundId = null) : ICommand<Guid>;
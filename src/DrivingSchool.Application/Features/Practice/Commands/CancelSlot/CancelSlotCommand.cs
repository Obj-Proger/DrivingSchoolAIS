using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CancelSlot;

/// <summary>
/// Cancels a practice slot. If the slot is booked, the booking
/// is cancelled by the instructor automatically.
/// </summary>
public sealed record CancelSlotCommand(Guid SlotId) : ICommand;
using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a student successfully books a practice slot.
/// Triggers a notification to the instructor.
/// </summary>
/// <param name="BookingId">The identifier of the newly created booking.</param>
/// <param name="SlotId">The identifier of the reserved slot.</param>
/// <param name="ContractId">The student contract under which the booking was made.</param>
public sealed record BookingCreatedEvent(
    Guid BookingId,
    Guid SlotId,
    Guid ContractId) : IDomainEvent;
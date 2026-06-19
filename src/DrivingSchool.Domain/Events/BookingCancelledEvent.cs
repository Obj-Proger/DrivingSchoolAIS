using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a practice booking is cancelled by either the student or the instructor.
/// Triggers slot release and notifications to both parties.
/// </summary>
/// <param name="BookingId">The identifier of the cancelled booking.</param>
/// <param name="SlotId">The identifier of the slot to be released.</param>
/// <param name="ContractId">The student contract associated with the booking.</param>
/// <param name="CancelledByStudent">
/// <c>true</c> if the student initiated the cancellation; <c>false</c> if the instructor did.
/// </param>
/// <param name="Reason">An optional reason for the cancellation.</param>
public sealed record BookingCancelledEvent(
    Guid BookingId,
    Guid SlotId,
    Guid ContractId,
    bool CancelledByStudent,
    string? Reason) : IDomainEvent;
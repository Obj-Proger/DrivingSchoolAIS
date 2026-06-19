using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when an instructor marks a practice lesson as completed.
/// Triggers a rating request notification to the student
/// and updates the contract's practice hours counter.
/// </summary>
/// <param name="BookingId">The identifier of the completed booking.</param>
/// <param name="ContractId">The student contract to update.</param>
/// <param name="HoursLogged">The number of practice hours recorded for this lesson.</param>
public sealed record BookingCompletedEvent(
    Guid BookingId,
    Guid ContractId,
    int HoursLogged) : IDomainEvent;
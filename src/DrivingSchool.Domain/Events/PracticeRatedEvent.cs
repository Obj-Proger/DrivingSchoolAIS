using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a student submits a rating for a completed practice lesson.
/// Triggers an update to the contract's quality indicator.
/// If the rating is below 4, the contract raises <see cref="LowRatingReceivedEvent"/>.
/// </summary>
/// <param name="BookingId">The identifier of the rated booking.</param>
/// <param name="ContractId">The student contract to update.</param>
/// <param name="Rating">The submitted rating value (1–5).</param>
public sealed record PracticeRatedEvent(
    Guid BookingId,
    Guid ContractId,
    int Rating) : IDomainEvent;
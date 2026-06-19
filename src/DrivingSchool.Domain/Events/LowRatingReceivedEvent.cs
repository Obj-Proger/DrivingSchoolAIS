using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a student gives a practice lesson a rating below 4.
/// Triggers a quality alert notification to all managers.
/// </summary>
/// <param name="ContractId">The contract under which the lesson was conducted.</param>
/// <param name="StudentId">The student who submitted the low rating.</param>
/// <param name="Rating">The submitted rating value (1–3).</param>
public sealed record LowRatingReceivedEvent(
    Guid ContractId,
    Guid StudentId,
    int Rating) : IDomainEvent;
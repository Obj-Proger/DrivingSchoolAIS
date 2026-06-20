using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a payment is confirmed by a manager.
/// Triggers the contract debt recalculation and a payment receipt notification
/// to the student.
/// </summary>
/// <param name="PaymentId">The identifier of the confirmed payment.</param>
/// <param name="ContractId">The contract to which the payment applies.</param>
/// <param name="Amount">The confirmed payment amount.</param>
public sealed record PaymentCompletedEvent(
    Guid PaymentId,
    Guid ContractId,
    Money Amount) : IDomainEvent;
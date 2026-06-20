using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Events;

/// <summary>
/// Raised when a payment is refunded.
/// Triggers the contract debt recalculation and a refund notification to the student.
/// </summary>
/// <param name="PaymentId">The identifier of the refunded payment.</param>
/// <param name="ContractId">The contract from which the payment is removed.</param>
/// <param name="Amount">The refunded amount.</param>
public sealed record PaymentRefundedEvent(
    Guid PaymentId,
    Guid ContractId,
    Money Amount) : IDomainEvent;
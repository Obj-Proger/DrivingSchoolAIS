using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Events;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a payment made by a student towards their training contract.
/// A payment is initially registered as pending and must be confirmed
/// by a manager before it is applied to the contract balance.
/// </summary>
public sealed class Payment : BaseEntity
{
    private Payment() { } // Required by EF Core

    /// <summary>Gets the identifier of the contract this payment is applied to.</summary>
    public Guid ContractId { get; private set; }

    /// <summary>Gets the identifier of the manager who registered this payment.</summary>
    public Guid ManagerId { get; private set; }

    /// <summary>Gets the payment amount.</summary>
    public Money Amount { get; private set; } = null!;

    /// <summary>
    /// Gets a human-readable description of what the payment covers
    /// (e.g. "First instalment", "Exam fee").
    /// </summary>
    public string Purpose { get; private set; } = string.Empty;

    /// <summary>Gets the payment method used by the student.</summary>
    public PaymentMethod Method { get; private set; }

    /// <summary>Gets the current processing status of this payment.</summary>
    public PaymentStatus Status { get; private set; }

    /// <summary>
    /// Gets the receipt or transaction reference number,
    /// or <c>null</c> if not yet confirmed.
    /// </summary>
    public string? ReceiptNumber { get; private set; }

    /// <summary>Gets the UTC timestamp when the payment was confirmed, or <c>null</c> if pending.</summary>
    public DateTime? PaidAt { get; private set; }

    // Factory

    /// <summary>
    /// Registers a new payment in the pending state.
    /// </summary>
    /// <param name="contractId">The contract the payment applies to.</param>
    /// <param name="managerId">The manager registering the payment.</param>
    /// <param name="amount">The payment amount.</param>
    /// <param name="purpose">A description of what the payment covers.</param>
    /// <param name="method">The payment method used.</param>
    public static Payment Create(
        Guid contractId,
        Guid managerId,
        Money amount,
        string purpose,
        PaymentMethod method)
        => new()
        {
            ContractId = contractId,
            ManagerId = managerId,
            Amount = amount,
            Purpose = purpose.Trim(),
            Method = method,
            Status = PaymentStatus.Pending
        };

    // Behaviour

    /// <summary>
    /// Confirms the payment, recording the optional receipt number.
    /// Raises <see cref="PaymentCompletedEvent"/> to trigger contract debt recalculation
    /// and a receipt notification to the student.
    /// </summary>
    /// <param name="receiptNumber">The optional receipt or transaction reference.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Payment.AlreadyProcessed"/>.
    /// </returns>
    public Result Confirm(string? receiptNumber = null)
    {
        if (Status != PaymentStatus.Pending)
            return Result.Failure(DomainErrors.Payment.AlreadyProcessed);

        Status = PaymentStatus.Completed;
        ReceiptNumber = receiptNumber?.Trim();
        PaidAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentCompletedEvent(Id, ContractId, Amount));

        return Result.Success();
    }

    /// <summary>
    /// Issues a refund for this payment.
    /// Raises <see cref="PaymentRefundedEvent"/> to trigger contract debt recalculation.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Payment.CannotRefund"/>.
    /// </returns>
    public Result Refund()
    {
        if (Status != PaymentStatus.Completed)
            return Result.Failure(DomainErrors.Payment.CannotRefund);

        Status = PaymentStatus.Refunded;

        RaiseDomainEvent(new PaymentRefundedEvent(Id, ContractId, Amount));

        return Result.Success();
    }
}
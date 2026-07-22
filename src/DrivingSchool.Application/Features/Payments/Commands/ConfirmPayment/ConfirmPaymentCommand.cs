using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Payments.Commands.ConfirmPayment;

/// <summary>
/// Confirms a pending payment. Raises <c>PaymentCompletedEvent</c>, which applies
/// the amount to the contract balance and notifies the student.
/// Restricted to managers and administrators.
/// </summary>
public sealed record ConfirmPaymentCommand(
    Guid PaymentId,
    string? ReceiptNumber = null) : ICommand;
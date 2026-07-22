using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Payments.Commands.RefundPayment;

/// <summary>
/// Refunds a completed payment. Raises <c>PaymentRefundedEvent</c>, which reverses
/// the amount on the contract balance and notifies the student.
/// Restricted to managers and administrators.
/// </summary>
public sealed record RefundPaymentCommand(Guid PaymentId) : ICommand;
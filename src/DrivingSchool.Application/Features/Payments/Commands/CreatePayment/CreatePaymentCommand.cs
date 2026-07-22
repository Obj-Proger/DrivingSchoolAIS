using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Payments.Commands.CreatePayment;

/// <summary>
/// Registers a new pending payment against a contract.
/// The current user is recorded as the registering manager.
/// Restricted to managers and administrators.
/// </summary>
public sealed record CreatePaymentCommand(
    Guid ContractId,
    decimal Amount,
    string Purpose,
    PaymentMethod Method) : ICommand<Guid>;
using DrivingSchool.Application.Features.Payments.DTOs;

namespace DrivingSchool.Application.Features.Payments.Queries.GetContractPayments;

/// <summary>Returns the full payment history for a contract, most recent first.</summary>
public sealed record GetContractPaymentsQuery(Guid ContractId)
    : IQuery<IReadOnlyList<PaymentDto>>;
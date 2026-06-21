namespace DrivingSchool.Application.Features.Contracts.DTOs;

/// <summary>
/// Lightweight debt summary used in the debtors list.
/// </summary>
public sealed record ContractDebtDto(
    Guid ContractId,
    string ContractNumber,
    Guid StudentId,
    string StudentName,
    string StudentPhone,
    decimal DebtAmount,
    DateTime? LastPaymentDate);
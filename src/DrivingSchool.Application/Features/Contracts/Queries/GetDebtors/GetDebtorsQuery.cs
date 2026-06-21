using DrivingSchool.Application.Features.Contracts.DTOs;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetDebtors;

/// <summary>
/// Returns all active contracts with an outstanding debt.
/// Used by managers to track and follow up on overdue payments.
/// </summary>
public sealed record GetDebtorsQuery : IQuery<IReadOnlyList<ContractDebtDto>>;
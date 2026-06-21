using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Contracts.DTOs;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetContracts;

/// <summary>
/// Returns a paginated list of contracts with optional filters.
/// Accessible to managers and administrators.
/// </summary>
public sealed record GetContractsQuery(
    int Page = 1,
    int PageSize = 20,
    ContractStatus? Status = null,
    Guid? StudentId = null,
    bool? HasDebt = null) : IQuery<PaginatedResult<ContractDto>>;
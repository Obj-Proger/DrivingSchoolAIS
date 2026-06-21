using DrivingSchool.Application.Features.Contracts.DTOs;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetContractById;

/// <summary>
/// Returns the full detail view of a single contract including payment history.
/// </summary>
public sealed record GetContractByIdQuery(Guid ContractId) : IQuery<ContractDetailDto>;
using DrivingSchool.Application.Features.Contracts.DTOs;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetStudentContracts;

/// <summary>
/// Returns all contracts belonging to the currently authenticated student.
/// </summary>
public sealed record GetStudentContractsQuery : IQuery<IReadOnlyList<ContractDto>>;
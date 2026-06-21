using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.ConvertLeadToContract;

/// <summary>
/// Converts a lead into a training contract, marking the lead as converted.
/// Returns the identifier of the newly created contract.
/// </summary>
public sealed record ConvertLeadToContractCommand(
    Guid LeadId,
    Guid CourseId,
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalCost) : ICommand<Guid>;
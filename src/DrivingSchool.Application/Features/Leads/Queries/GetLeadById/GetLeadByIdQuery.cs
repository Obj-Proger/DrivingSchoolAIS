using DrivingSchool.Application.Features.Leads.DTOs;

namespace DrivingSchool.Application.Features.Leads.Queries.GetLeadById;

/// <summary>
/// Returns the full detail view of a single lead including its note history.
/// </summary>
public sealed record GetLeadByIdQuery(Guid LeadId) : IQuery<LeadDetailDto>;
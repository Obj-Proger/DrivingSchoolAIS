using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Leads.DTOs;

namespace DrivingSchool.Application.Features.Leads.Queries.GetLeads;

/// <summary>
/// Returns a paginated, filterable list of leads for the CRM view.
/// </summary>
public sealed record GetLeadsQuery(
    int Page = 1,
    int PageSize = 20,
    LeadStatus? Status = null,
    LeadSource? Source = null,
    Guid? ResponsibleManagerId = null,
    string? Search = null) : IQuery<PaginatedResult<LeadDto>>;
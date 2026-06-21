using DrivingSchool.Application.Features.Leads.DTOs;

namespace DrivingSchool.Application.Features.Leads.Queries.GetLeadStats;

/// <summary>
/// Returns aggregated CRM statistics for the manager analytics dashboard.
/// </summary>
public sealed record GetLeadStatsQuery : IQuery<LeadStatsDto>;
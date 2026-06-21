namespace DrivingSchool.Application.Features.Leads.DTOs;

/// <summary>
/// Aggregated lead statistics used in the CRM analytics dashboard.
/// </summary>
public sealed record LeadStatsDto(
    int TotalLeads,
    IReadOnlyList<LeadCountBySource> BySource,
    IReadOnlyList<LeadCountByStatus> ByStatus,
    decimal ConversionRate);

/// <summary>Number of leads per acquisition source.</summary>
public sealed record LeadCountBySource(LeadSource Source, int Count);

/// <summary>Number of leads per pipeline status.</summary>
public sealed record LeadCountByStatus(LeadStatus Status, int Count);
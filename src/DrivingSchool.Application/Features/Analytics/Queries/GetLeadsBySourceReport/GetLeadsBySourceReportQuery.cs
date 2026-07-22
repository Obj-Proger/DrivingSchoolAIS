using DrivingSchool.Application.Features.Analytics.DTOs;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetLeadsBySourceReport;

/// <summary>
/// Returns lead volume and conversion rate broken down by acquisition source,
/// for leads created within the specified date range.
/// Restricted to managers and administrators.
/// </summary>
public sealed record GetLeadsBySourceReportQuery(DateTime From, DateTime To)
    : IQuery<IReadOnlyList<LeadSourceReportItemDto>>;
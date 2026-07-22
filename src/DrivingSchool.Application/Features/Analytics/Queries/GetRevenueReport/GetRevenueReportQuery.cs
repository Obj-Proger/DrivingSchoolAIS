using DrivingSchool.Application.Features.Analytics.DTOs;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetRevenueReport;

/// <summary>
/// Returns confirmed revenue over a date range, broken down by month.
/// Restricted to managers and administrators.
/// </summary>
public sealed record GetRevenueReportQuery(DateTime From, DateTime To)
    : IQuery<RevenueReportDto>;
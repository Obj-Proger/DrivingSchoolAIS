using DrivingSchool.Application.Features.Analytics.DTOs;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetContractsSummaryReport;

/// <summary>
/// Returns contract volume by status and a success signal (completion rate,
/// average quality indicator) for contracts signed within the date range.
/// Restricted to managers and administrators.
/// </summary>
public sealed record GetContractsSummaryReportQuery(DateTime From, DateTime To)
    : IQuery<ContractsSummaryReportDto>;
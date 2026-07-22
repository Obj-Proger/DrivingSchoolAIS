namespace DrivingSchool.Application.Features.Analytics.DTOs;

/// <summary>
/// Outcome summary for contracts signed within a date range — combines contract
/// volume by status with a quality/success signal (completion rate and average
/// rolling quality indicator), since both describe the same underlying cohort.
/// </summary>
public sealed record ContractsSummaryReportDto(
    int TotalCount,
    int ActiveCount,
    int CompletedCount,
    int TerminatedCount,
    double CompletionRatePercent,
    double? AverageQualityIndicator);
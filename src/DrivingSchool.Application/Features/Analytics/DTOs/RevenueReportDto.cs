namespace DrivingSchool.Application.Features.Analytics.DTOs;

/// <summary>Confirmed revenue over a date range, broken down by month.</summary>
public sealed record RevenueReportDto(
    decimal TotalRevenue,
    IReadOnlyList<MonthlyRevenueDto> ByMonth);

/// <summary>Confirmed revenue for a single calendar month.</summary>
public sealed record MonthlyRevenueDto(int Year, int Month, decimal Total);
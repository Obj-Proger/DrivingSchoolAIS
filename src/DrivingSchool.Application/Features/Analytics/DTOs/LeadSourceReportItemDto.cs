namespace DrivingSchool.Application.Features.Analytics.DTOs;

/// <summary>Lead volume and conversion rate broken down by acquisition source.</summary>
public sealed record LeadSourceReportItemDto(
    LeadSource Source,
    int TotalCount,
    int ConvertedCount,
    double ConversionRatePercent);
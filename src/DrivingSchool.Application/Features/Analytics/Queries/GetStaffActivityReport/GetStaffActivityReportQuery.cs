using DrivingSchool.Application.Features.Analytics.DTOs;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetStaffActivityReport;

/// <summary>
/// Returns per-staff-member task activity — assigned, completed, and overdue
/// counts — for tasks created within the date range.
/// Restricted to managers and administrators.
/// </summary>
public sealed record GetStaffActivityReportQuery(DateTime From, DateTime To)
    : IQuery<IReadOnlyList<StaffActivityItemDto>>;
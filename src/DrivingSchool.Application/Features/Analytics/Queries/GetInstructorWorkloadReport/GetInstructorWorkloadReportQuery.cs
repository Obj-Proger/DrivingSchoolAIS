using DrivingSchool.Application.Features.Analytics.DTOs;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetInstructorWorkloadReport;

/// <summary>
/// Returns per-instructor booking workload for slots starting within the
/// date range — booking volume, completion/cancellation counts, hours
/// logged, and average student rating.
/// Restricted to managers and administrators.
/// </summary>
public sealed record GetInstructorWorkloadReportQuery(DateTime From, DateTime To)
    : IQuery<IReadOnlyList<InstructorWorkloadItemDto>>;
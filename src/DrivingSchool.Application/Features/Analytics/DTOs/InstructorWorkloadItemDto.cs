namespace DrivingSchool.Application.Features.Analytics.DTOs;

/// <summary>Workload and outcome statistics for a single instructor within a date range.</summary>
public sealed record InstructorWorkloadItemDto(
    Guid InstructorId,
    string InstructorName,
    int TotalBookings,
    int CompletedBookings,
    int CancelledBookings,
    int TotalHoursLogged,
    double? AverageRating);
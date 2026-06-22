using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetAvailableSlots;

/// <summary>
/// Returns all available practice slots within the specified date range.
/// Used by students when booking a lesson.
/// </summary>
public sealed record GetAvailableSlotsQuery(
    DateTime From,
    DateTime To,
    Guid? InstructorId = null,
    LicenseCategory? Category = null) : IQuery<IReadOnlyList<PracticeSlotDto>>;
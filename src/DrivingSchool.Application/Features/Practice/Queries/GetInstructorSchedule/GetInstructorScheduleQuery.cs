using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetInstructorSchedule;

/// <summary>
/// Returns the full schedule of slots for an instructor within a date range,
/// including booking information for each booked slot.
/// </summary>
public sealed record GetInstructorScheduleQuery(
    Guid InstructorId,
    DateTime From,
    DateTime To) : IQuery<IReadOnlyList<PracticeSlotDetailDto>>;
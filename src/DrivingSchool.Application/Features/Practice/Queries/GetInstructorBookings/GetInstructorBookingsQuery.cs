using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetInstructorBookings;

/// <summary>
/// Returns all bookings for slots owned by the specified instructor
/// within a date range. Used in the instructor's student list view.
/// </summary>
public sealed record GetInstructorBookingsQuery(
    Guid InstructorId,
    DateTime From,
    DateTime To) : IQuery<IReadOnlyList<BookingDto>>;
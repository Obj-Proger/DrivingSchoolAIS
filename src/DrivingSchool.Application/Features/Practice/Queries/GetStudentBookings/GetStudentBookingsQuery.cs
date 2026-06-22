using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetStudentBookings;

/// <summary>
/// Returns the booking history for the current student's contract.
/// </summary>
public sealed record GetStudentBookingsQuery(
    Guid ContractId,
    BookingStatus? Status = null,
    DateTime? From = null,
    DateTime? To = null) : IQuery<IReadOnlyList<BookingDto>>;
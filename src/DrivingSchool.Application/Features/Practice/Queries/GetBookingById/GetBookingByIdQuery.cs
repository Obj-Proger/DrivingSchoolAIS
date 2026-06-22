using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Queries.GetBookingById;

/// <summary>Returns the full detail view of a single booking.</summary>
public sealed record GetBookingByIdQuery(Guid BookingId) : IQuery<BookingDto>;
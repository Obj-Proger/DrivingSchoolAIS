using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Practice.DTOs;

namespace DrivingSchool.Application.Features.Practice.Commands.CompleteBooking;

/// <summary>
/// Marks a booking as completed after the practical lesson has been delivered.
/// Records skill ratings, driving route, instructor note, and hours driven.
/// </summary>
public sealed record CompleteBookingCommand(
    Guid BookingId,
    IReadOnlyList<SkillRatingRequest> SkillRatings,
    int PracticeHoursLogged,
    Guid? RouteId = null,
    string? InstructorNote = null) : ICommand;
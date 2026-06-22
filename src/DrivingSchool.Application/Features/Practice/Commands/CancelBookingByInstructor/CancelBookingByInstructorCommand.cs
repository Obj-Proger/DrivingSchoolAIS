using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CancelBookingByInstructor;

/// <summary>Cancels a confirmed booking on behalf of the instructor.</summary>
public sealed record CancelBookingByInstructorCommand(
    Guid BookingId,
    string? Reason = null) : ICommand;
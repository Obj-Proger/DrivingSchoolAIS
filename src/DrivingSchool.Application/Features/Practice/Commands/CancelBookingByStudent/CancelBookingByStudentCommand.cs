using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CancelBookingByStudent;

/// <summary>Cancels a confirmed booking on behalf of the student.</summary>
public sealed record CancelBookingByStudentCommand(
    Guid BookingId,
    string? Reason = null) : ICommand;
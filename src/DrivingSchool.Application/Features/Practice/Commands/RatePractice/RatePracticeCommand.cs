using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.RatePractice;

/// <summary>
/// Records the student's rating for a completed practice lesson.
/// Triggers the quality indicator update on the contract via domain event.
/// </summary>
public sealed record RatePracticeCommand(
    Guid BookingId,
    int Rating,
    string? Review = null) : ICommand;
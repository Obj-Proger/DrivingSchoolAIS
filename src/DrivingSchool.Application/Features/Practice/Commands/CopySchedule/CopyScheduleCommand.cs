using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Practice.Commands.CopySchedule;

/// <summary>
/// Copies all Available slots from a source date to a target date,
/// preserving the time-of-day for each slot.
/// Returns the number of slots created.
/// </summary>
public sealed record CopyScheduleCommand(
    DateTime SourceDate,
    DateTime TargetDate) : ICommand<int>;
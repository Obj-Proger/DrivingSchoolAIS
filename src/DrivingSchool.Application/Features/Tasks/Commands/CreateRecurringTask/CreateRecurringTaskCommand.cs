using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Tasks.Commands.CreateRecurringTask;

/// <summary>
/// Creates a recurring task that repeats automatically on the specified days of the week.
/// </summary>
public sealed record CreateRecurringTaskCommand(
    string Title,
    string? Description,
    Guid AssignedToId,
    TaskPriority Priority,
    IReadOnlyList<DayOfWeek> RecurringDays) : ICommand<Guid>;
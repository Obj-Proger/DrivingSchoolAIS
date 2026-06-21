using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Tasks.Commands.CreateTask;

/// <summary>Creates a one-time task and assigns it to a staff member.</summary>
public sealed record CreateTaskCommand(
    string Title,
    string? Description,
    Guid AssignedToId,
    TaskPriority Priority,
    DateTime? DueDate = null,
    string? LinkedEntityType = null,
    Guid? LinkedEntityId = null) : ICommand<Guid>;
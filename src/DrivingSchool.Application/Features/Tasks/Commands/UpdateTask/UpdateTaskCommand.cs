using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Tasks.Commands.UpdateTask;

/// <summary>Updates the title, description, and due date of a task.</summary>
public sealed record UpdateTaskCommand(
    Guid TaskId,
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate) : ICommand;
using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Tasks.Commands.CompleteTask;

/// <summary>Marks a task as completed.</summary>
public sealed record CompleteTaskCommand(Guid TaskId) : ICommand;
using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Tasks.Commands.DeleteTask;

/// <summary>Permanently removes a task from the system.</summary>
public sealed record DeleteTaskCommand(Guid TaskId) : ICommand;
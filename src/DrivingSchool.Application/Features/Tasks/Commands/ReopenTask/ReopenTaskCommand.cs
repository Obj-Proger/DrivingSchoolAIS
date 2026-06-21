using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Tasks.Commands.ReopenTask;

/// <summary>Reopens a completed task, resetting it to New status.</summary>
public sealed record ReopenTaskCommand(Guid TaskId) : ICommand;
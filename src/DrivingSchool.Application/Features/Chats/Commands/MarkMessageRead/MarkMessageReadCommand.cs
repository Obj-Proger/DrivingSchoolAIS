using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.MarkMessageRead;

/// <summary>
/// Records that the current user has read a specific message and notifies
/// other participants in real time. The client calls this once per message
/// as it becomes visible on screen (the standard read-receipt pattern).
/// </summary>
public sealed record MarkMessageReadCommand(Guid MessageId) : ICommand;
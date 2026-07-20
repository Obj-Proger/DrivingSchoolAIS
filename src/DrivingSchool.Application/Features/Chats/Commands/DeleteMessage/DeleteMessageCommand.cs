using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.DeleteMessage;

/// <summary>
/// Soft-deletes a message. A user may only delete their own messages.
/// </summary>
public sealed record DeleteMessageCommand(Guid MessageId) : ICommand;
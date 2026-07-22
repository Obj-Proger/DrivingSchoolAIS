using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.RemoveChatParticipant;

/// <summary>
/// Removes a user from a group chat. A chat administrator may remove anyone;
/// any participant may remove themselves (leave the chat).
/// </summary>
public sealed record RemoveChatParticipantCommand(Guid ChatId, Guid UserId) : ICommand;
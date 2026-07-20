using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.CreateGroupChat;

/// <summary>
/// Creates a group chat, broadcast channel, or read-only channel.
/// The current user becomes the chat's creator and first administrator.
/// </summary>
public sealed record CreateGroupChatCommand(
    string Name,
    ChatType Type,
    IReadOnlyList<Guid> ParticipantIds) : ICommand<Guid>;
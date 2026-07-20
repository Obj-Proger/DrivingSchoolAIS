using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.MuteChat;

/// <summary>Mutes or unmutes push notifications for the current user in a chat.</summary>
public sealed record MuteChatCommand(Guid ChatId, bool Mute) : ICommand;
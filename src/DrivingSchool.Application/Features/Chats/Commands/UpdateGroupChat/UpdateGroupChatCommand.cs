using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.UpdateGroupChat;

/// <summary>Renames a group chat. Restricted to chat administrators.</summary>
public sealed record UpdateGroupChatCommand(Guid ChatId, string Name) : ICommand;
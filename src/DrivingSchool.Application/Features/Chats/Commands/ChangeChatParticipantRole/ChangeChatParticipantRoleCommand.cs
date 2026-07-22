using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.ChangeChatParticipantRole;

/// <summary>Promotes or demotes a chat participant. Restricted to chat administrators.</summary>
public sealed record ChangeChatParticipantRoleCommand(
    Guid ChatId,
    Guid UserId,
    ParticipantRole NewRole) : ICommand;
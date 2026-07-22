using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.AddChatParticipant;

/// <summary>Adds a user to a group chat.</summary>
public sealed record AddChatParticipantCommand(
    Guid ChatId,
    Guid UserId,
    ParticipantRole Role = ParticipantRole.Member) : ICommand;
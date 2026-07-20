using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Chats.DTOs;

namespace DrivingSchool.Application.Features.Chats.Commands.SendMessage;

/// <summary>
/// Sends a text message to a chat. The text is encrypted at rest (AES-256-GCM)
/// and broadcast in real time, decrypted, to every connected participant.
/// </summary>
public sealed record SendMessageCommand(Guid ChatId, string Text) : ICommand<MessageDto>;
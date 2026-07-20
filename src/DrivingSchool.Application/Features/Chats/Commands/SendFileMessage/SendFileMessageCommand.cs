using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Chats.DTOs;

namespace DrivingSchool.Application.Features.Chats.Commands.SendFileMessage;

/// <summary>
/// Uploads a file or image and sends it as a message. The original file name
/// is encrypted at rest; the file itself is stored via <c>IFileStorageService</c>.
/// </summary>
public sealed record SendFileMessageCommand(
    Guid ChatId,
    Stream FileStream,
    string FileName,
    string ContentType,
    MessageContentType Kind) : ICommand<MessageDto>;
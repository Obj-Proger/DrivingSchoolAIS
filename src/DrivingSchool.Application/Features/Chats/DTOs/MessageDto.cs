namespace DrivingSchool.Application.Features.Chats.DTOs;

/// <summary>
/// A single message with its content already decrypted for display.
/// Never persisted or logged in this decrypted form.
/// </summary>
public sealed record MessageDto(
    Guid Id,
    Guid ChatId,
    Guid? SenderId,
    string? SenderName,
    string? SenderPhotoUrl,
    string Content,
    MessageContentType ContentType,
    string? FileUrl,
    string? FileName,
    long? FileSizeBytes,
    SystemAction? SystemAction,
    string? ActionPayload,
    DateTime SentAt,
    bool IsDelivered,
    bool IsReadByMe);
namespace DrivingSchool.Application.Features.Chats.DTOs;

/// <summary>
/// Summary representation of a chat used in the conversation list.
/// For <see cref="ChatType.Direct"/> chats, <see cref="Name"/> and <see cref="PhotoUrl"/>
/// are derived from the other participant rather than stored on the chat itself.
/// </summary>
public sealed record ChatSummaryDto(
    Guid Id,
    ChatType Type,
    string Name,
    string? PhotoUrl,
    bool IsSystem,
    bool IsMuted,
    int UnreadCount,
    string? LastMessagePreview,
    DateTime? LastMessageAt);
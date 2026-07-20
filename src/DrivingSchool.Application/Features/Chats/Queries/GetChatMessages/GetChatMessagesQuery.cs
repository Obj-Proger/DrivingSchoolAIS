using DrivingSchool.Application.Features.Chats.DTOs;

namespace DrivingSchool.Application.Features.Chats.Queries.GetChatMessages;

/// <summary>
/// Returns a page of decrypted messages for a chat using cursor-based pagination.
/// </summary>
/// <param name="ChatId">The chat identifier.</param>
/// <param name="Before">
/// Cursor: only messages sent before this UTC timestamp are returned.
/// Leave <c>null</c> to retrieve the most recent messages.
/// </param>
/// <param name="Limit">Maximum number of messages to return.</param>
public sealed record GetChatMessagesQuery(
    Guid ChatId,
    DateTime? Before = null,
    int Limit = 50) : IQuery<IReadOnlyList<MessageDto>>;
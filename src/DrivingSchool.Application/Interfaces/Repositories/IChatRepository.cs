namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Chat"/> aggregate
/// and its associated <see cref="Message"/> entities.
/// </summary>
public interface IChatRepository
{
    // Chats

    /// <summary>Returns the chat with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Chat?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the direct chat between two users, or <c>null</c> if none exists.
    /// </summary>
    Task<Chat?> GetDirectChatAsync(Guid user1Id, Guid user2Id, CancellationToken ct = default);

    /// <summary>
    /// Returns all chats in which the specified user participates,
    /// with the most recently updated chat first.
    /// </summary>
    Task<IReadOnlyList<Chat>> GetUserChatsAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Returns <c>true</c> if the specified user is a participant in the given chat.
    /// </summary>
    Task<bool> IsParticipantAsync(Guid chatId, Guid userId, CancellationToken ct = default);

    /// <summary>Adds a new chat to the repository.</summary>
    Task AddAsync(Chat chat, CancellationToken ct = default);

    /// <summary>Marks an existing chat as modified.</summary>
    void Update(Chat chat);

    // Messages

    /// <summary>
    /// Returns a page of messages for the specified chat using cursor-based pagination.
    /// Messages are ordered by <see cref="Message.SentAt"/> descending.
    /// </summary>
    /// <param name="chatId">The chat identifier.</param>
    /// <param name="before">
    /// Cursor: only messages sent before this UTC timestamp are returned.
    /// Pass <c>null</c> to retrieve the most recent messages.
    /// </param>
    /// <param name="limit">Maximum number of messages to return.</param>
    Task<IReadOnlyList<Message>> GetMessagesAsync(
        Guid chatId,
        DateTime? before,
        int limit,
        CancellationToken ct = default);

    /// <summary>Returns the number of unread messages for the user in the specified chat.</summary>
    Task<int> GetUnreadCountAsync(Guid chatId, Guid userId, CancellationToken ct = default);

    /// <summary>Returns the message with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Message?> GetMessageByIdAsync(Guid messageId, CancellationToken ct = default);

    /// <summary>Adds a new message to the repository.</summary>
    Task AddMessageAsync(Message message, CancellationToken ct = default);

    /// <summary>Marks an existing message as modified.</summary>
    void UpdateMessage(Message message);
}
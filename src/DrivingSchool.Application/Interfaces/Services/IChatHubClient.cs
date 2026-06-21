namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Defines the strongly-typed methods callable on the SignalR chat hub client.
/// This interface is implemented by the browser/mobile client and called
/// by the server via <c>IHubContext&lt;ChatHub, IChatHubClient&gt;</c>.
/// </summary>
public interface IChatHubClient
{
    /// <summary>
    /// Pushes a new decrypted message to the client.
    /// Called when a message is sent in a chat the client has joined.
    /// </summary>
    Task ReceiveMessage(ChatMessagePayload message);

    /// <summary>
    /// Notifies the client that a specific message has been read by a user.
    /// </summary>
    Task MessageRead(Guid chatId, Guid messageId, Guid userId, DateTime readAt);

    /// <summary>Broadcasts the online/offline status change of a user.</summary>
    Task UserOnline(Guid userId, bool isOnline);

    /// <summary>Indicates whether a user is currently typing in a chat.</summary>
    Task UserTyping(Guid chatId, Guid userId, bool isTyping);

    /// <summary>
    /// Pushes a system notification (title + body) to the client outside of a chat context.
    /// </summary>
    Task SystemNotification(string title, string body, NotificationType type);
}

/// <summary>
/// Data transfer object for a message delivered over SignalR.
/// Contains the decrypted text for the recipient.
/// </summary>
public sealed record ChatMessagePayload(
    Guid Id,
    Guid ChatId,
    Guid? SenderId,
    string? SenderName,
    string? SenderPhotoUrl,
    string Content,
    MessageContentType ContentType,
    string? FileUrl,
    string? FileName,
    SystemAction? SystemAction,
    string? ActionPayload,
    DateTime SentAt);
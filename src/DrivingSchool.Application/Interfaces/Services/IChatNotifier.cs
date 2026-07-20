namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Broadcasts real-time chat events to connected clients.
/// Implemented in the API layer as a thin wrapper around
/// <c>IHubContext&lt;ChatHub, IChatHubClient&gt;</c>, keeping the Application
/// layer free of any direct dependency on SignalR.
/// </summary>
public interface IChatNotifier
{
    /// <summary>Pushes a newly sent message to every client currently in the chat.</summary>
    Task NotifyMessageSentAsync(
        Guid chatId,
        ChatMessagePayload message,
        CancellationToken ct = default);

    /// <summary>Notifies chat members that a message has been read by a user.</summary>
    Task NotifyMessageReadAsync(
        Guid chatId,
        Guid messageId,
        Guid userId,
        DateTime readAt,
        CancellationToken ct = default);

    /// <summary>Notifies chat members that a user started or stopped typing.</summary>
    Task NotifyTypingAsync(
        Guid chatId,
        Guid userId,
        bool isTyping,
        CancellationToken ct = default);
}
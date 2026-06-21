namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Creates and broadcasts automated system messages into chats
/// in response to domain events.
/// </summary>
public interface ISystemMessageService
{
    /// <summary>
    /// Creates an encrypted system message in the specified chat,
    /// broadcasts it over SignalR, and persists it to the database.
    /// </summary>
    /// <param name="chatId">The target chat identifier.</param>
    /// <param name="action">The domain action this message represents.</param>
    /// <param name="actionPayload">
    /// A JSON string with the entity identifiers needed to build the client-side deep link.
    /// </param>
    /// <param name="displayText">The human-readable message text (encrypted before storage).</param>
    Task SendAsync(
        Guid chatId,
        SystemAction action,
        string actionPayload,
        string displayText,
        CancellationToken ct = default);
}
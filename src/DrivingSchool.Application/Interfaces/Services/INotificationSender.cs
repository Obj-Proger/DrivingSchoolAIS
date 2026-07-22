namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Sends real-time and persisted notifications to users.
/// The concrete implementation dispatches via SignalR and persists
/// a <see cref="Notification"/> record in the database.
/// </summary>
public interface INotificationSender
{
    /// <summary>
    /// Sends a notification to a specific user and persists it in the database.
    /// </summary>
    Task SendToUserAsync(
        Guid userId,
        string title,
        string body,
        NotificationType type,
        string? actionUrl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a notification to all members of the specified group
    /// and persists a record for each member.
    /// </summary>
    Task SendToGroupMembersAsync(
        Guid groupId,
        string title,
        string body,
        NotificationType type,
        string? actionUrl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a notification to all users with the specified role
    /// (e.g. all managers when a low quality alert is triggered).
    /// </summary>
    Task SendToRoleAsync(
        UserRole role,
        string title,
        string body,
        NotificationType type,
        string? actionUrl = null,
        CancellationToken cancellationToken = default);
}
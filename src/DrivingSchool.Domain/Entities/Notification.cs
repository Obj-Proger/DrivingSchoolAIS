using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents an in-app notification delivered to a specific user.
/// Notifications are created by domain event handlers and displayed
/// in the notification centre of the web and mobile clients.
/// </summary>
public sealed class Notification : BaseEntity
{
    private Notification() { } // Required by EF Core

    /// <summary>Gets the identifier of the user this notification is addressed to.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the short notification headline.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets the full notification body text.</summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>Gets the category of this notification.</summary>
    public NotificationType Type { get; private set; }

    /// <summary>Gets a value indicating whether the user has read this notification.</summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// Gets a deep-link URL that navigates the client to the relevant entity
    /// when the notification is tapped, or <c>null</c> if no navigation is required.
    /// </summary>
    public string? ActionUrl { get; private set; }

    // Factory

    /// <summary>
    /// Creates a new unread notification for the specified user.
    /// </summary>
    /// <param name="userId">The recipient's user identifier.</param>
    /// <param name="title">The notification headline.</param>
    /// <param name="body">The notification body text.</param>
    /// <param name="type">The notification category.</param>
    /// <param name="actionUrl">An optional deep-link URL.</param>
    public static Notification Create(
        Guid userId,
        string title,
        string body,
        NotificationType type,
        string? actionUrl = null)
        => new()
        {
            UserId = userId,
            Title = title.Trim(),
            Body = body.Trim(),
            Type = type,
            ActionUrl = actionUrl,
            IsRead = false
        };

    // Behaviour

    /// <summary>Marks the notification as read.</summary>
    public void MarkRead() => IsRead = true;
}
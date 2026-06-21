using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Notification"/> entity.
/// </summary>
public interface INotificationRepository
{
    /// <summary>Returns a paginated list of notifications for the specified user.</summary>
    Task<PaginatedResult<Notification>> GetByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        bool? isRead = null,
        CancellationToken ct = default);

    /// <summary>Returns the number of unread notifications for the specified user.</summary>
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Returns the notification with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Adds a new notification to the repository.</summary>
    Task AddAsync(Notification notification, CancellationToken ct = default);

    /// <summary>Marks an existing notification as modified.</summary>
    void Update(Notification notification);

    /// <summary>
    /// Marks all unread notifications for the specified user as read
    /// in a single database operation.
    /// </summary>
    Task MarkAllReadAsync(Guid userId, CancellationToken ct = default);
}
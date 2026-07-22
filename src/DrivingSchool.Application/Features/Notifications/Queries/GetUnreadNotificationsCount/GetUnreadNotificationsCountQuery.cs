namespace DrivingSchool.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;

/// <summary>
/// Returns the number of unread notifications for the current user,
/// used for the notification bell badge.
/// </summary>
public sealed record GetUnreadNotificationsCountQuery : IQuery<int>;
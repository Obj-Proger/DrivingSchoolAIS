using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Notifications.DTOs;

namespace DrivingSchool.Application.Features.Notifications.Queries.GetUserNotifications;

/// <summary>
/// Returns a paginated list of the current user's notifications,
/// most recent first.
/// </summary>
public sealed record GetUserNotificationsQuery(
    int Page = 1,
    int PageSize = 20,
    bool? IsRead = null) : IQuery<PaginatedResult<NotificationDto>>;
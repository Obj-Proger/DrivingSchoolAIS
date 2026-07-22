using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Notifications.Commands.MarkAllNotificationsRead;

/// <summary>Marks every unread notification for the current user as read.</summary>
public sealed record MarkAllNotificationsReadCommand : ICommand;
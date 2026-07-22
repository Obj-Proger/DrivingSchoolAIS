using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Notifications.Commands.MarkNotificationRead;

/// <summary>Marks a single notification as read.</summary>
public sealed record MarkNotificationReadCommand(Guid NotificationId) : ICommand;
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>Notifies every student in the group when their theory lesson is cancelled.</summary>
internal sealed class LessonCancelledEventHandler
    : INotificationHandler<DomainEventNotification<LessonCancelledEvent>>
{
    private readonly INotificationSender _notificationSender;

    public LessonCancelledEventHandler(INotificationSender notificationSender)
        => _notificationSender = notificationSender;

    public async Task Handle(
        DomainEventNotification<LessonCancelledEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var body = $"The lesson scheduled for " +
                   $"{domainEvent.ScheduledAt:dd MMMM yyyy, HH:mm} was cancelled: " +
                   $"{domainEvent.Reason}";

        await _notificationSender.SendToGroupMembersAsync(
            domainEvent.GroupId,
            "Lesson cancelled",
            body,
            NotificationType.LessonCancelled,
            actionUrl: $"/theory/lessons/{domainEvent.LessonId}",
            cancellationToken: cancellationToken);
    }
}
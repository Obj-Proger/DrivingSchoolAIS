using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>Alerts all managers when a student submits a low practice-lesson rating.</summary>
internal sealed class LowRatingReceivedEventHandler
    : INotificationHandler<DomainEventNotification<LowRatingReceivedEvent>>
{
    private readonly INotificationSender _notificationSender;

    public LowRatingReceivedEventHandler(INotificationSender notificationSender)
        => _notificationSender = notificationSender;

    public async Task Handle(
        DomainEventNotification<LowRatingReceivedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _notificationSender.SendToRoleAsync(
            UserRole.Manager,
            "Low quality alert",
            $"A student submitted a rating of {domainEvent.Rating}/5 for a practice lesson.",
            NotificationType.LowQualityAlert,
            actionUrl: $"/contracts/{domainEvent.ContractId}",
            cancellationToken: cancellationToken);
    }
}
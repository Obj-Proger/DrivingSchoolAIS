using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Notifies every student in the group and the responsible teacher
/// when a new exam is scheduled.
/// </summary>
internal sealed class ExamScheduledEventHandler
    : INotificationHandler<DomainEventNotification<ExamScheduledEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationSender _notificationSender;

    public ExamScheduledEventHandler(
        IUnitOfWork unitOfWork,
        INotificationSender notificationSender)
    {
        _unitOfWork = unitOfWork;
        _notificationSender = notificationSender;
    }

    public async Task Handle(
        DomainEventNotification<ExamScheduledEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var group = await _unitOfWork.Groups
            .GetByIdAsync(domainEvent.GroupId, cancellationToken);

        if (group is null) return;

        var typeLabel = domainEvent.Type == ExamType.Theory ? "theory" : "practical";
        var body = $"A {typeLabel} exam has been scheduled for " +
                   $"{domainEvent.ScheduledAt:dd MMMM yyyy, HH:mm}.";

        // Notify the teacher
        await _notificationSender.SendToUserAsync(
            group.TeacherId,
            "Exam scheduled",
            body,
            NotificationType.ExamScheduled,
            actionUrl: $"/exams/{domainEvent.ExamEventId}",
            cancellationToken);

        // Notify every student in the group
        await _notificationSender.SendToGroupMembersAsync(
            domainEvent.GroupId,
            "Exam scheduled",
            body,
            NotificationType.ExamScheduled,
            actionUrl: $"/exams/{domainEvent.ExamEventId}",
            cancellationToken: cancellationToken);
    }
}
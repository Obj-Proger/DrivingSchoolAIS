using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Notifies the instructor when a student books one of their practice slots.
/// </summary>
internal sealed class BookingCreatedEventHandler
    : INotificationHandler<DomainEventNotification<BookingCreatedEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationSender _notificationSender;

    public BookingCreatedEventHandler(
        IUnitOfWork unitOfWork,
        INotificationSender notificationSender)
    {
        _unitOfWork = unitOfWork;
        _notificationSender = notificationSender;
    }

    public async Task Handle(
        DomainEventNotification<BookingCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(domainEvent.SlotId, cancellationToken);

        if (slot is null) return;

        await _notificationSender.SendToUserAsync(
            slot.InstructorId,
            "New practice booking",
            "A student has booked one of your practice slots.",
            NotificationType.PracticeBooked,
            actionUrl: $"/practice/bookings/{domainEvent.BookingId}",
            cancellationToken);
    }
}
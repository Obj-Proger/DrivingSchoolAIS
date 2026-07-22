using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Releases the slot so it can be booked again, and notifies whichever party
/// did not initiate the cancellation.
/// </summary>
internal sealed class BookingCancelledEventHandler
    : INotificationHandler<DomainEventNotification<BookingCancelledEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationSender _notificationSender;

    public BookingCancelledEventHandler(
        IUnitOfWork unitOfWork,
        INotificationSender notificationSender)
    {
        _unitOfWork = unitOfWork;
        _notificationSender = notificationSender;
    }

    public async Task Handle(
        DomainEventNotification<BookingCancelledEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(domainEvent.SlotId, cancellationToken);

        if (slot is null) return;

        var releaseResult = slot.Release();
        if (releaseResult.IsSuccess)
        {
            _unitOfWork.PracticeSlots.Update(slot);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var body = string.IsNullOrWhiteSpace(domainEvent.Reason)
            ? "A practice booking has been cancelled."
            : $"A practice booking has been cancelled: {domainEvent.Reason}";

        if (domainEvent.CancelledByStudent)
        {
            // Notify the instructor
            await _notificationSender.SendToUserAsync(
                slot.InstructorId,
                "Booking cancelled",
                body,
                NotificationType.PracticeCancelled,
                cancellationToken: cancellationToken);
        }
        else
        {
            // Notify the student
            var contract = await _unitOfWork.Contracts
                .GetByIdAsync(domainEvent.ContractId, cancellationToken);

            if (contract is not null)
            {
                await _notificationSender.SendToUserAsync(
                    contract.StudentId,
                    "Booking cancelled",
                    body,
                    NotificationType.PracticeCancelled,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
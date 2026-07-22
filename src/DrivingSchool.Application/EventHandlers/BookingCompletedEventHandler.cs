using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Updates the contract's practice-hours counter and asks the student
/// to rate the completed lesson.
/// </summary>
internal sealed class BookingCompletedEventHandler
    : INotificationHandler<DomainEventNotification<BookingCompletedEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationSender _notificationSender;

    public BookingCompletedEventHandler(
        IUnitOfWork unitOfWork,
        INotificationSender notificationSender)
    {
        _unitOfWork = unitOfWork;
        _notificationSender = notificationSender;
    }

    public async Task Handle(
        DomainEventNotification<BookingCompletedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(domainEvent.ContractId, cancellationToken);

        if (contract is null) return;

        contract.RegisterPracticeHours(domainEvent.HoursLogged);

        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _notificationSender.SendToUserAsync(
            contract.StudentId,
            "Rate your lesson",
            "Your practice lesson is complete. Let us know how it went.",
            NotificationType.RatingRequest,
            actionUrl: $"/practice/bookings/{domainEvent.BookingId}/rate",
            cancellationToken);
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Applies a confirmed payment to the contract's balance and sends
/// a receipt notification to the student.
/// </summary>
internal sealed class PaymentCompletedEventHandler
    : INotificationHandler<DomainEventNotification<PaymentCompletedEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationSender _notificationSender;

    public PaymentCompletedEventHandler(
        IUnitOfWork unitOfWork,
        INotificationSender notificationSender)
    {
        _unitOfWork = unitOfWork;
        _notificationSender = notificationSender;
    }

    public async Task Handle(
        DomainEventNotification<PaymentCompletedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(domainEvent.ContractId, cancellationToken);

        if (contract is null) return;

        contract.RecordPayment(domainEvent.Amount);

        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _notificationSender.SendToUserAsync(
            contract.StudentId,
            "Payment received",
            $"Your payment of {domainEvent.Amount.Amount} {domainEvent.Amount.Currency} " +
            "has been confirmed. Thank you!",
            NotificationType.PaymentReceived,
            actionUrl: $"/contracts/{domainEvent.ContractId}/payments",
            cancellationToken);
    }
}
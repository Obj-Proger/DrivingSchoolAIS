using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Reverses a refunded payment on the contract's balance and notifies the student.
/// </summary>
internal sealed class PaymentRefundedEventHandler
    : INotificationHandler<DomainEventNotification<PaymentRefundedEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationSender _notificationSender;

    public PaymentRefundedEventHandler(
        IUnitOfWork unitOfWork,
        INotificationSender notificationSender)
    {
        _unitOfWork = unitOfWork;
        _notificationSender = notificationSender;
    }

    public async Task Handle(
        DomainEventNotification<PaymentRefundedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(domainEvent.ContractId, cancellationToken);

        if (contract is null) return;

        contract.ReversePayment(domainEvent.Amount);

        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _notificationSender.SendToUserAsync(
            contract.StudentId,
            "Payment refunded",
            $"A refund of {domainEvent.Amount.Amount} {domainEvent.Amount.Currency} " +
            "has been processed on your contract.",
            NotificationType.PaymentRefunded,
            actionUrl: $"/contracts/{domainEvent.ContractId}/payments",
            cancellationToken);
    }
}
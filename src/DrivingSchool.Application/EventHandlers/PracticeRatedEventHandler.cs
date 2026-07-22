using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Updates the contract's rolling quality indicator with the student's rating.
/// If the rating is below 4, <see cref="Contract.RecordRating"/> raises
/// <see cref="LowRatingReceivedEvent"/> itself, handled separately by
/// <see cref="LowRatingReceivedEventHandler"/> — no notification is sent here.
/// </summary>
internal sealed class PracticeRatedEventHandler
    : INotificationHandler<DomainEventNotification<PracticeRatedEvent>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PracticeRatedEventHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task Handle(
        DomainEventNotification<PracticeRatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(domainEvent.ContractId, cancellationToken);

        if (contract is null) return;

        contract.RecordRating(domainEvent.Rating);

        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
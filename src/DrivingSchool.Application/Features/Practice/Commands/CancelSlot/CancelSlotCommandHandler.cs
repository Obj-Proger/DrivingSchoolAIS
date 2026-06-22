using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.CancelSlot;

/// <summary>Handles <see cref="CancelSlotCommand"/>.</summary>
internal sealed class CancelSlotCommandHandler : ICommandHandler<CancelSlotCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelSlotCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CancelSlotCommand command,
        CancellationToken cancellationToken)
    {
        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(command.SlotId, cancellationToken);

        if (slot is null)
            return Result.Failure(DomainErrors.PracticeSlot.NotFound);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        // Cancel any active booking on this slot first
        if (slot.Status == SlotStatus.Booked)
        {
            var booking = await _unitOfWork.PracticeBookings
                .GetBySlotIdAsync(slot.Id, cancellationToken);

            if (booking is not null)
            {
                var cancelBookingResult = booking.CancelByInstructor(
                    reason: "The slot was cancelled by the instructor.");

                if (cancelBookingResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return cancelBookingResult;
                }

                _unitOfWork.PracticeBookings.Update(booking);
            }
        }

        var cancelResult = slot.Cancel();
        if (cancelResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return cancelResult;
        }

        _unitOfWork.PracticeSlots.Update(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}
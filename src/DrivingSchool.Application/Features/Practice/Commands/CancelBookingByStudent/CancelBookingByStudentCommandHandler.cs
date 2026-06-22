using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.CancelBookingByStudent;

/// <summary>Handles <see cref="CancelBookingByStudentCommand"/>.</summary>
internal sealed class CancelBookingByStudentCommandHandler
    : ICommandHandler<CancelBookingByStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelBookingByStudentCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CancelBookingByStudentCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.PracticeBookings
            .GetByIdAsync(command.BookingId, cancellationToken);

        if (booking is null)
            return Result.Failure(DomainErrors.PracticeBooking.NotFound);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var cancelResult = booking.CancelByStudent(command.Reason);
        if (cancelResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return cancelResult;
        }

        // Release the slot back to Available
        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(booking.SlotId, cancellationToken);

        if (slot is not null)
        {
            slot.Release();
            _unitOfWork.PracticeSlots.Update(slot);
        }

        _unitOfWork.PracticeBookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}
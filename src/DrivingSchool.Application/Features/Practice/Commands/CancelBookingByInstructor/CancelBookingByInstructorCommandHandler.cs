using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.CancelBookingByInstructor;

/// <summary>Handles <see cref="CancelBookingByInstructorCommand"/>.</summary>
internal sealed class CancelBookingByInstructorCommandHandler
    : ICommandHandler<CancelBookingByInstructorCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelBookingByInstructorCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CancelBookingByInstructorCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.PracticeBookings
            .GetByIdAsync(command.BookingId, cancellationToken);

        if (booking is null)
            return Result.Failure(DomainErrors.PracticeBooking.NotFound);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var cancelResult = booking.CancelByInstructor(command.Reason);
        if (cancelResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return cancelResult;
        }

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
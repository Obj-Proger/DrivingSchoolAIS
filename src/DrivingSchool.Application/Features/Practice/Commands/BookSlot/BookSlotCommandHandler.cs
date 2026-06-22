using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.BookSlot;

/// <summary>
/// Handles <see cref="BookSlotCommand"/>.
/// Validates slot availability, overlap with existing student bookings,
/// then creates the booking and marks the slot as booked.
/// </summary>
internal sealed class BookSlotCommandHandler : ICommandHandler<BookSlotCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public BookSlotCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        BookSlotCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Validate slot
        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(command.SlotId, cancellationToken);

        if (slot is null)
            return Result.Failure<Guid>(DomainErrors.PracticeSlot.NotFound);

        if (!slot.CanBeBooked)
            return Result.Failure<Guid>(DomainErrors.PracticeSlot.NotAvailable);

        // 2. Validate contract
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(command.ContractId, cancellationToken);

        if (contract is null)
            return Result.Failure<Guid>(DomainErrors.Contract.NotFound);

        if (contract.Status != ContractStatus.Active)
            return Result.Failure<Guid>(DomainErrors.Contract.AlreadyClosed);

        // 3. Check student has no overlapping booking
        var hasOverlap = await _unitOfWork.PracticeBookings.HasOverlapAsync(
            command.ContractId,
            slot.StartDateTime,
            slot.EndDateTime,
            cancellationToken);

        if (hasOverlap)
            return Result.Failure<Guid>(Error.Conflict("PracticeBooking"));

        // 4. Validate ground choice
        Guid? groundId = null;
        if (command.SelectedTrainingGroundId.HasValue)
        {
            if (!slot.IsOpenForStudentGroundChoice)
                return Result.Failure<Guid>(DomainErrors.PracticeSlot.NotAvailable);
            groundId = command.SelectedTrainingGroundId;
        }
        else
        {
            groundId = slot.DefaultTrainingGroundId;
        }

        // 5. Book slot and create booking atomically
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var bookResult = slot.Book();
        if (bookResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Failure<Guid>(bookResult.Error);
        }

        var booking = PracticeBooking.Create(command.SlotId, command.ContractId, groundId);

        _unitOfWork.PracticeSlots.Update(slot);
        await _unitOfWork.PracticeBookings.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success(booking.Id);
    }
}
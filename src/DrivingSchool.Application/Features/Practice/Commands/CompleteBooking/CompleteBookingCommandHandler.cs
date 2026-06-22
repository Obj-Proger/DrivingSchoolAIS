using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.CompleteBooking;

/// <summary>
/// Handles <see cref="CompleteBookingCommand"/>.
/// Completes the booking, logs practice hours on the contract,
/// and marks the slot as completed.
/// </summary>
internal sealed class CompleteBookingCommandHandler : ICommandHandler<CompleteBookingCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CompleteBookingCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CompleteBookingCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Load booking
        var booking = await _unitOfWork.PracticeBookings
            .GetByIdAsync(command.BookingId, cancellationToken);

        if (booking is null)
            return Result.Failure(DomainErrors.PracticeBooking.NotFound);

        // 2. Build skill ratings from domain
        var skillRatings = new List<SkillRating>();
        foreach (var req in command.SkillRatings)
        {
            var ratingResult = SkillRating.Create(booking.Id, req.SkillName, req.Score);
            if (ratingResult.IsFailure) return Result.Failure(ratingResult.Error);
            skillRatings.Add(ratingResult.Value);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        // 3. Complete the booking
        var completeResult = booking.Complete(
            skillRatings,
            command.PracticeHoursLogged,
            command.RouteId,
            command.InstructorNote);

        if (completeResult.IsFailure)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return completeResult;
        }

        // 4. Log hours on the contract
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(booking.ContractId, cancellationToken);

        if (contract is not null)
        {
            contract.RegisterPracticeHours(command.PracticeHoursLogged);
            _unitOfWork.Contracts.Update(contract);
        }

        // 5. Mark slot as completed
        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(booking.SlotId, cancellationToken);

        if (slot is not null)
        {
            slot.Complete();
            _unitOfWork.PracticeSlots.Update(slot);
        }

        _unitOfWork.PracticeBookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}
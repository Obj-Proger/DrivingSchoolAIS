using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.RatePractice;

/// <summary>
/// Handles <see cref="RatePracticeCommand"/>.
/// Records the student rating on the booking and updates the contract quality indicator.
/// </summary>
internal sealed class RatePracticeCommandHandler : ICommandHandler<RatePracticeCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RatePracticeCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        RatePracticeCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.PracticeBookings
            .GetByIdAsync(command.BookingId, cancellationToken);

        if (booking is null)
            return Result.Failure(DomainErrors.PracticeBooking.NotFound);

        // Rate the booking — raises PracticeRatedEvent
        var rateResult = booking.Rate(command.Rating, command.Review);
        if (rateResult.IsFailure) return rateResult;

        // Update quality indicator on contract
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(booking.ContractId, cancellationToken);

        if (contract is not null)
        {
            // RecordRating raises LowRatingReceivedEvent if rating < 4
            contract.RecordRating(command.Rating);
            _unitOfWork.Contracts.Update(contract);
        }

        _unitOfWork.PracticeBookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
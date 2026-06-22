using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.UpdateSlot;

/// <summary>Handles <see cref="UpdateSlotCommand"/>.</summary>
internal sealed class UpdateSlotCommandHandler : ICommandHandler<UpdateSlotCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSlotCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateSlotCommand command,
        CancellationToken cancellationToken)
    {
        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(command.SlotId, cancellationToken);

        if (slot is null)
            return Result.Failure(DomainErrors.PracticeSlot.NotFound);

        // Validate no overlap with other slots (excluding self)
        var hasOverlap = await _unitOfWork.PracticeSlots.HasOverlapAsync(
            slot.InstructorId,
            command.StartDateTime,
            command.EndDateTime,
            excludeSlotId: command.SlotId,
            ct: cancellationToken);

        if (hasOverlap)
            return Result.Failure(Error.Conflict("PracticeSlot"));

        var result = slot.Update(
            command.StartDateTime,
            command.EndDateTime,
            command.VehicleId,
            command.DefaultTrainingGroundId,
            command.Note);

        if (result.IsFailure) return result;

        _unitOfWork.PracticeSlots.Update(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
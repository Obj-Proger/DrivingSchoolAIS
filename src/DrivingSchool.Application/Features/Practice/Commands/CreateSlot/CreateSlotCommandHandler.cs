using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Practice.Commands.CreateSlot;

/// <summary>
/// Handles <see cref="CreateSlotCommand"/>.
/// Validates that the instructor has no overlapping slots before creating.
/// </summary>
internal sealed class CreateSlotCommandHandler : ICommandHandler<CreateSlotCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateSlotCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateSlotCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Check for overlapping slots
        var hasOverlap = await _unitOfWork.PracticeSlots.HasOverlapAsync(
            _currentUser.UserId,
            command.StartDateTime,
            command.EndDateTime,
            ct: cancellationToken);

        if (hasOverlap)
            return Result.Failure<Guid>(Error.Conflict("PracticeSlot"));

        // 2. Create slot aggregate
        var slotResult = PracticeSlot.Create(
            _currentUser.UserId,
            command.StartDateTime,
            command.EndDateTime,
            command.Type,
            command.VehicleId,
            command.DefaultTrainingGroundId,
            command.IsOpenForStudentGroundChoice,
            command.Note);

        if (slotResult.IsFailure)
            return Result.Failure<Guid>(slotResult.Error);

        // 3. Persist
        await _unitOfWork.PracticeSlots.AddAsync(slotResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(slotResult.Value.Id);
    }
}
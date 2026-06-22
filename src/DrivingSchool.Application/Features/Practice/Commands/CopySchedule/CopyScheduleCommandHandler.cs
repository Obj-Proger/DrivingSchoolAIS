using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Practice.Commands.CopySchedule;

/// <summary>
/// Handles <see cref="CopyScheduleCommand"/>.
/// Copies all Available slots from the source day to the target day.
/// Skips any slot that would overlap with an existing slot on the target day.
/// </summary>
internal sealed class CopyScheduleCommandHandler : ICommandHandler<CopyScheduleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CopyScheduleCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(
        CopyScheduleCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Load source day slots
        var sourceSlots = await _unitOfWork.PracticeSlots.GetInstructorScheduleAsync(
            _currentUser.UserId,
            command.SourceDate.Date,
            command.SourceDate.Date.AddDays(1),
            cancellationToken);

        var availableSlots = sourceSlots
            .Where(s => s.Status == SlotStatus.Available)
            .ToList();

        if (availableSlots.Count == 0)
            return Result.Success(0);

        // 2. Calculate day offset
        var dayDiff = command.TargetDate.Date - command.SourceDate.Date;
        var newSlots = new List<PracticeSlot>();

        foreach (var source in availableSlots)
        {
            var newStart = source.StartDateTime + dayDiff;
            var newEnd = source.EndDateTime + dayDiff;

            // Skip if would create overlap
            var hasOverlap = await _unitOfWork.PracticeSlots.HasOverlapAsync(
                _currentUser.UserId,
                newStart,
                newEnd,
                ct: cancellationToken);

            if (hasOverlap) continue;

            var slotResult = PracticeSlot.Create(
                _currentUser.UserId,
                newStart,
                newEnd,
                source.Type,
                source.VehicleId,
                source.DefaultTrainingGroundId,
                source.IsOpenForStudentGroundChoice,
                source.Note);

            if (slotResult.IsSuccess)
                newSlots.Add(slotResult.Value);
        }

        // 3. Persist all new slots in a single operation
        if (newSlots.Count > 0)
        {
            await _unitOfWork.PracticeSlots.AddRangeAsync(newSlots, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success(newSlots.Count);
    }
}
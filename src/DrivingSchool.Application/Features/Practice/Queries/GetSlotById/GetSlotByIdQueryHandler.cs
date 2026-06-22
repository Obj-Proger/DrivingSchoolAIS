using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Features.Practice.Queries.GetAvailableSlots;
using DrivingSchool.Application.Features.Practice.Queries.GetInstructorSchedule;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetSlotById;

/// <summary>Handles <see cref="GetSlotByIdQuery"/>.</summary>
internal sealed class GetSlotByIdQueryHandler
    : IQueryHandler<GetSlotByIdQuery, PracticeSlotDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetInstructorScheduleQueryHandler _scheduleHandler;

    public GetSlotByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _scheduleHandler = new GetInstructorScheduleQueryHandler(unitOfWork);
    }

    public async Task<Result<PracticeSlotDetailDto>> Handle(
        GetSlotByIdQuery query,
        CancellationToken cancellationToken)
    {
        var slot = await _unitOfWork.PracticeSlots
            .GetByIdAsync(query.SlotId, cancellationToken);

        if (slot is null)
            return Result.Failure<PracticeSlotDetailDto>(DomainErrors.PracticeSlot.NotFound);

        // Reuse the schedule handler which resolves booking data
        var scheduleResult = await _scheduleHandler.Handle(
            new GetInstructorScheduleQuery(
                slot.InstructorId,
                slot.StartDateTime.Date,
                slot.StartDateTime.Date.AddDays(1)),
            cancellationToken);

        var slotDetail = scheduleResult.Value
            .FirstOrDefault(s => s.Id == query.SlotId);

        if (slotDetail is null)
            return Result.Failure<PracticeSlotDetailDto>(DomainErrors.PracticeSlot.NotFound);

        return Result.Success(slotDetail);
    }
}
using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Features.Practice.Queries.GetAvailableSlots;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetInstructorSchedule;

/// <summary>
/// Handles <see cref="GetInstructorScheduleQuery"/>.
/// </summary>
internal sealed class GetInstructorScheduleQueryHandler
    : IQueryHandler<GetInstructorScheduleQuery, IReadOnlyList<PracticeSlotDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetAvailableSlotsQueryHandler _slotMapper;

    public GetInstructorScheduleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _slotMapper = new GetAvailableSlotsQueryHandler(unitOfWork);
    }

    public async Task<Result<IReadOnlyList<PracticeSlotDetailDto>>> Handle(
        GetInstructorScheduleQuery query,
        CancellationToken cancellationToken)
    {
        var slots = await _unitOfWork.PracticeSlots.GetInstructorScheduleAsync(
            query.InstructorId,
            query.From,
            query.To,
            cancellationToken);

        var dtos = new List<PracticeSlotDetailDto>();

        foreach (var slot in slots)
        {
            var slotDto = await _slotMapper.MapToDtoAsync(slot, cancellationToken);

            // Resolve booking if slot is booked
            BookingBriefDto? bookingBrief = null;
            if (slot.Status == SlotStatus.Booked)
            {
                var booking = await _unitOfWork.PracticeBookings
                    .GetBySlotIdAsync(slot.Id, cancellationToken);

                if (booking is not null)
                {
                    var contract = await _unitOfWork.Contracts
                        .GetByIdAsync(booking.ContractId, cancellationToken);

                    if (contract is not null)
                    {
                        var student = await _unitOfWork.Users
                            .GetByIdAsync(contract.StudentId, cancellationToken);

                        bookingBrief = new BookingBriefDto(
                            booking.Id,
                            booking.ContractId,
                            contract.StudentId,
                            student?.FullName.DisplayName ?? "Unknown",
                            student?.Phone.Value ?? string.Empty,
                            booking.Status);
                    }
                }
            }

            dtos.Add(new PracticeSlotDetailDto(
                slotDto.Id,
                slotDto.InstructorId,
                slotDto.InstructorName,
                slotDto.InstructorPhotoUrl,
                slotDto.VehicleId,
                slotDto.VehicleModel,
                slotDto.VehiclePlate,
                slotDto.DefaultTrainingGroundId,
                slotDto.DefaultGroundName,
                slotDto.StartDateTime,
                slotDto.EndDateTime,
                slotDto.DurationMinutes,
                slotDto.Type,
                slotDto.Status,
                slotDto.IsOpenForStudentGroundChoice,
                slotDto.Note,
                bookingBrief));
        }

        return Result.Success<IReadOnlyList<PracticeSlotDetailDto>>(dtos);
    }
}
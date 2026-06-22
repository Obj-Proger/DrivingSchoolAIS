using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetAvailableSlots;

/// <summary>
/// Handles <see cref="GetAvailableSlotsQuery"/>.
/// </summary>
internal sealed class GetAvailableSlotsQueryHandler
    : IQueryHandler<GetAvailableSlotsQuery, IReadOnlyList<PracticeSlotDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAvailableSlotsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<PracticeSlotDto>>> Handle(
        GetAvailableSlotsQuery query,
        CancellationToken cancellationToken)
    {
        var slots = await _unitOfWork.PracticeSlots.GetAvailableAsync(
            query.From,
            query.To,
            query.InstructorId,
            query.Category,
            cancellationToken);

        var dtos = new List<PracticeSlotDto>();

        foreach (var slot in slots)
        {
            var dto = await MapToDtoAsync(slot, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success<IReadOnlyList<PracticeSlotDto>>(dtos);
    }

    internal async Task<PracticeSlotDto> MapToDtoAsync(
        PracticeSlot slot,
        CancellationToken ct)
    {
        var instructor = await _unitOfWork.Users.GetByIdAsync(slot.InstructorId, ct);

        string? vehicleModel = null;
        string? vehiclePlate = null;
        if (slot.VehicleId.HasValue)
        {
            // Vehicle resolution delegated to Infrastructure via enrichment
            // Left as null here — resolved by a decorated handler or projection
        }

        string? groundName = null;
        if (slot.DefaultTrainingGroundId.HasValue)
        {
            // Ground name resolution delegated to Infrastructure
        }

        return new PracticeSlotDto(
            slot.Id,
            slot.InstructorId,
            instructor?.FullName.ShortName ?? "Unknown",
            instructor?.PhotoUrl,
            slot.VehicleId,
            vehicleModel,
            vehiclePlate,
            slot.DefaultTrainingGroundId,
            groundName,
            slot.StartDateTime,
            slot.EndDateTime,
            slot.DurationMinutes,
            slot.Type,
            slot.Status,
            slot.IsOpenForStudentGroundChoice,
            slot.Note);
    }
}
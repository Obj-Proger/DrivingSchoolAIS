using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetStudentBookings;

/// <summary>Handles <see cref="GetStudentBookingsQuery"/>.</summary>
internal sealed class GetStudentBookingsQueryHandler
    : IQueryHandler<GetStudentBookingsQuery, IReadOnlyList<BookingDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentBookingsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<BookingDto>>> Handle(
        GetStudentBookingsQuery query,
        CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.PracticeBookings
            .GetByContractIdAsync(query.ContractId, query.Status, cancellationToken);

        var dtos = new List<BookingDto>();

        foreach (var booking in bookings)
        {
            var dto = await MapToBookingDtoAsync(booking, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success<IReadOnlyList<BookingDto>>(dtos);
    }

    internal async Task<BookingDto> MapToBookingDtoAsync(
        PracticeBooking booking,
        CancellationToken ct)
    {
        var slot = await _unitOfWork.PracticeSlots.GetByIdAsync(booking.SlotId, ct);
        var instructor = slot is not null
            ? await _unitOfWork.Users.GetByIdAsync(slot.InstructorId, ct)
            : null;

        string? groundName = null;
        if (booking.SelectedTrainingGroundId.HasValue)
        {
            var ground = await _unitOfWork.TrainingGrounds
                .GetByIdAsync(booking.SelectedTrainingGroundId.Value, ct);
            groundName = ground?.Name;
        }

        string? routeName = null;
        if (booking.RouteId.HasValue)
        {
            var route = await _unitOfWork.DrivingRoutes
                .GetByIdAsync(booking.RouteId.Value, ct);
            routeName = route?.Name;
        }

        var skillDtos = booking.SkillRatings
            .Select(r => new SkillRatingDto(r.SkillName, r.Score))
            .ToList();

        return new BookingDto(
            booking.Id,
            booking.SlotId,
            booking.ContractId,
            slot?.InstructorId ?? Guid.Empty,
            instructor?.FullName.ShortName ?? "Unknown",
            instructor?.PhotoUrl,
            slot?.StartDateTime ?? default,
            slot?.EndDateTime ?? default,
            booking.SelectedTrainingGroundId,
            groundName,
            null, // vehicle model — no IVehicleRepository yet
            null, // vehicle plate — no IVehicleRepository yet
            booking.Status,
            booking.StudentRating,
            booking.StudentReview,
            booking.RouteId,
            routeName,
            booking.InstructorNote,
            booking.PracticeHoursLogged,
            skillDtos,
            booking.BookedAt);
    }
}
using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Features.Practice.Queries.GetStudentBookings;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetInstructorBookings;

/// <summary>Handles <see cref="GetInstructorBookingsQuery"/>.</summary>
internal sealed class GetInstructorBookingsQueryHandler
    : IQueryHandler<GetInstructorBookingsQuery, IReadOnlyList<BookingDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetStudentBookingsQueryHandler _bookingMapper;

    public GetInstructorBookingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _bookingMapper = new GetStudentBookingsQueryHandler(unitOfWork);
    }

    public async Task<Result<IReadOnlyList<BookingDto>>> Handle(
        GetInstructorBookingsQuery query,
        CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.PracticeBookings.GetByInstructorAsync(
            query.InstructorId,
            query.From,
            query.To,
            cancellationToken);

        var dtos = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            var dto = await _bookingMapper.MapToBookingDtoAsync(booking, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success<IReadOnlyList<BookingDto>>(dtos);
    }
}
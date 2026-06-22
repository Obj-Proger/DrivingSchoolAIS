using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Features.Practice.Queries.GetStudentBookings;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetBookingById;

/// <summary>Handles <see cref="GetBookingByIdQuery"/>.</summary>
internal sealed class GetBookingByIdQueryHandler
    : IQueryHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetStudentBookingsQueryHandler _mapper;

    public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new GetStudentBookingsQueryHandler(unitOfWork);
    }

    public async Task<Result<BookingDto>> Handle(
        GetBookingByIdQuery query,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.PracticeBookings
            .GetByIdAsync(query.BookingId, cancellationToken);

        if (booking is null)
            return Result.Failure<BookingDto>(DomainErrors.PracticeBooking.NotFound);

        var dto = await _mapper.MapToBookingDtoAsync(booking, cancellationToken);
        return Result.Success(dto);
    }
}
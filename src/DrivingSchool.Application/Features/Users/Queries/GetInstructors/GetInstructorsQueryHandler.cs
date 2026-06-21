using DrivingSchool.Application.Features.Users.DTOs;
using DrivingSchool.Application.Features.Users.Queries.GetAllUsers;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Queries.GetInstructors;

/// <summary>
/// Handles <see cref="GetInstructorsQuery"/>.
/// </summary>
internal sealed class GetInstructorsQueryHandler
    : IQueryHandler<GetInstructorsQuery, IReadOnlyList<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstructorsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<UserDto>>> Handle(
        GetInstructorsQuery query,
        CancellationToken cancellationToken)
    {
        var instructors = await _unitOfWork.Users
            .GetInstructorsAsync(cancellationToken);

        var dtos = instructors
            .Select(GetAllUsersQueryHandler.MapToDto)
            .ToList();

        return Result.Success<IReadOnlyList<UserDto>>(dtos);
    }
}
using DrivingSchool.Application.Features.Users.DTOs;
using DrivingSchool.Application.Features.Users.Queries.GetAllUsers;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Handles <see cref="GetUserByIdQuery"/>.
/// </summary>
internal sealed class GetUserByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(query.UserId, cancellationToken);

        if (user is null)
            return Result.Failure<UserDto>(DomainErrors.User.NotFound);

        return Result.Success(GetAllUsersQueryHandler.MapToDto(user));
    }
}
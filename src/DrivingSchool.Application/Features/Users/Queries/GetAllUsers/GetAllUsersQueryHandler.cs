using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Users.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Queries.GetAllUsers;

/// <summary>
/// Handles <see cref="GetAllUsersQuery"/>.
/// </summary>
internal sealed class GetAllUsersQueryHandler
    : IQueryHandler<GetAllUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<UserDto>>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Users.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            query.Search,
            query.Role,
            query.IsActive,
            cancellationToken);

        var dtos = paginated.Items.Select(MapToDto).ToList();

        return Result.Success(new PaginatedResult<UserDto>(
            dtos,
            paginated.TotalCount,
            paginated.Page,
            paginated.PageSize));
    }

    internal static UserDto MapToDto(User user) => new(
        user.Id,
        user.FullName.FirstName,
        user.FullName.LastName,
        user.FullName.MiddleName,
        user.FullName.DisplayName,
        user.FullName.ShortName,
        user.Email.Value,
        user.Phone.Value,
        user.Role,
        user.PhotoUrl,
        user.IsActive,
        user.IsEmailConfirmed,
        user.LastLoginAt,
        user.CreatedAt,
        user.BranchId);
}
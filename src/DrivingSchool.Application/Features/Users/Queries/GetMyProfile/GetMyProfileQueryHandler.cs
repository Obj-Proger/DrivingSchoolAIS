using DrivingSchool.Application.Features.Users.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Users.Queries.GetMyProfile;

/// <summary>
/// Handles <see cref="GetMyProfileQuery"/>.
/// </summary>
internal sealed class GetMyProfileQueryHandler
    : IQueryHandler<GetMyProfileQuery, UserProfileDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetMyProfileQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<UserProfileDto>> Handle(
        GetMyProfileQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByIdAsync(_currentUser.UserId, cancellationToken);

        if (user is null)
            return Result.Failure<UserProfileDto>(DomainErrors.User.NotFound);

        var contracts = await _unitOfWork.Contracts
            .GetByStudentIdAsync(user.Id, cancellationToken);

        return Result.Success(new UserProfileDto(
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
            contracts.Select(c => c.Id).ToList()));
    }
}
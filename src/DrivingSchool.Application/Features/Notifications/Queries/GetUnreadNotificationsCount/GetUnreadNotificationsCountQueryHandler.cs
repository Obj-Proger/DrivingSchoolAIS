using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;

/// <summary>Handles <see cref="GetUnreadNotificationsCountQuery"/>.</summary>
internal sealed class GetUnreadNotificationsCountQueryHandler
    : IQueryHandler<GetUnreadNotificationsCountQuery, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetUnreadNotificationsCountQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(
        GetUnreadNotificationsCountQuery query,
        CancellationToken cancellationToken)
    {
        var count = await _unitOfWork.Notifications
            .GetUnreadCountAsync(_currentUser.UserId, cancellationToken);

        return Result.Success(count);
    }
}
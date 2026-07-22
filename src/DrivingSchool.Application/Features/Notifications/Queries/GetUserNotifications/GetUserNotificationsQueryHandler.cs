using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Notifications.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Notifications.Queries.GetUserNotifications;

/// <summary>Handles <see cref="GetUserNotificationsQuery"/>.</summary>
internal sealed class GetUserNotificationsQueryHandler
    : IQueryHandler<GetUserNotificationsQuery, PaginatedResult<NotificationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetUserNotificationsQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<PaginatedResult<NotificationDto>>> Handle(
        GetUserNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Notifications.GetByUserIdAsync(
            _currentUser.UserId,
            query.Page,
            query.PageSize,
            query.IsRead,
            cancellationToken);

        var dtos = paginated.Items
            .Select(n => new NotificationDto(
                n.Id,
                n.Title,
                n.Body,
                n.Type,
                n.IsRead,
                n.ActionUrl,
                n.CreatedAt))
            .ToList();

        return Result.Success(new PaginatedResult<NotificationDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Notifications.Commands.MarkAllNotificationsRead;

/// <summary>Handles <see cref="MarkAllNotificationsReadCommand"/>.</summary>
internal sealed class MarkAllNotificationsReadCommandHandler
    : ICommandHandler<MarkAllNotificationsReadCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public MarkAllNotificationsReadCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        MarkAllNotificationsReadCommand command,
        CancellationToken cancellationToken)
    {
        await _unitOfWork.Notifications
            .MarkAllReadAsync(_currentUser.UserId, cancellationToken);

        return Result.Success();
    }
}
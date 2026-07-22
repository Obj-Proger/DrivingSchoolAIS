using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Notifications.Commands.MarkNotificationRead;

/// <summary>Handles <see cref="MarkNotificationReadCommand"/>.</summary>
internal sealed class MarkNotificationReadCommandHandler
    : ICommandHandler<MarkNotificationReadCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public MarkNotificationReadCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        MarkNotificationReadCommand command,
        CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.Notifications
            .GetByIdAsync(command.NotificationId, cancellationToken);

        // Treat another user's notification the same as "not found" —
        // existence should not be disclosed to a user who does not own it.
        if (notification is null || notification.UserId != _currentUser.UserId)
            return Result.Failure(DomainErrors.Notification.NotFound);

        notification.MarkRead();

        _unitOfWork.Notifications.Update(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Notifies a user by email that their account has been deactivated.
/// Active sessions are already revoked synchronously inside
/// <see cref="User.Deactivate"/> — this handler only sends the notice.
/// </summary>
internal sealed class UserDeactivatedEventHandler
    : INotificationHandler<DomainEventNotification<UserDeactivatedEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public UserDeactivatedEventHandler(
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(
        DomainEventNotification<UserDeactivatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var user = await _unitOfWork.Users
            .GetByIdAsync(domainEvent.UserId, cancellationToken);

        if (user is null) return;

        await _emailService.SendTemplatedAsync(
            to: user.Email.Value,
            templateName: "AccountDeactivated",
            variables: new Dictionary<string, string>
            {
                ["UserName"] = user.FullName.FirstName
            },
            cancellationToken);
    }
}
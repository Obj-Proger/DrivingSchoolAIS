using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Application.EventHandlers;

/// <summary>
/// Sends the email confirmation link after a new account is registered.
/// This is the single place this email is sent from — command handlers that
/// create users (e.g. <c>RegisterCommandHandler</c>) must not send it directly,
/// to avoid delivering the confirmation email twice.
/// </summary>
internal sealed class UserRegisteredEventHandler
    : INotificationHandler<DomainEventNotification<UserRegisteredEvent>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public UserRegisteredEventHandler(
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(
        DomainEventNotification<UserRegisteredEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // Re-fetch the persisted user to read the confirmation token generated
        // inside User.Create — the event payload intentionally does not carry
        // the token itself.
        var user = await _unitOfWork.Users
            .GetByIdAsync(domainEvent.UserId, cancellationToken);

        if (user?.EmailConfirmationToken is null) return;

        await _emailService.SendTemplatedAsync(
            to: domainEvent.Email,
            templateName: "EmailConfirmation",
            variables: new Dictionary<string, string>
            {
                ["UserName"] = domainEvent.FullName,
                ["ConfirmationToken"] = user.EmailConfirmationToken
            },
            cancellationToken);
    }
}
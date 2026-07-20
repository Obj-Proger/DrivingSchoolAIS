using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.MarkMessageRead;

/// <summary>Handles <see cref="MarkMessageReadCommand"/>.</summary>
internal sealed class MarkMessageReadCommandHandler : ICommandHandler<MarkMessageReadCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IChatNotifier _chatNotifier;

    public MarkMessageReadCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IChatNotifier chatNotifier)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _chatNotifier = chatNotifier;
    }

    public async Task<Result> Handle(
        MarkMessageReadCommand command,
        CancellationToken cancellationToken)
    {
        var message = await _unitOfWork.Chats
            .GetMessageByIdAsync(command.MessageId, cancellationToken);

        if (message is null)
            return Result.Failure(DomainErrors.Message.NotFound);

        message.MarkReadBy(_currentUser.UserId);

        _unitOfWork.Chats.UpdateMessage(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var readAt = message.ReadBy
            .First(r => r.UserId == _currentUser.UserId)
            .ReadAt;

        await _chatNotifier.NotifyMessageReadAsync(
            message.ChatId, message.Id, _currentUser.UserId, readAt, cancellationToken);

        return Result.Success();
    }
}
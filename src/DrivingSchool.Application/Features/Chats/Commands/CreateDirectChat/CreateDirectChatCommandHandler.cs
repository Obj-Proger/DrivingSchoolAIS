using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.CreateDirectChat;

/// <summary>Handles <see cref="CreateDirectChatCommand"/>.</summary>
internal sealed class CreateDirectChatCommandHandler
    : ICommandHandler<CreateDirectChatCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateDirectChatCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateDirectChatCommand command,
        CancellationToken cancellationToken)
    {
        if (command.RecipientId == _currentUser.UserId)
            return Result.Failure<Guid>(
                new Error("Chat.CannotMessageSelf", "You cannot start a chat with yourself."));

        var recipient = await _unitOfWork.Users
            .GetByIdAsync(command.RecipientId, cancellationToken);

        if (recipient is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound);

        var existingChat = await _unitOfWork.Chats.GetDirectChatAsync(
            _currentUser.UserId, command.RecipientId, cancellationToken);

        if (existingChat is not null)
            return Result.Success(existingChat.Id);

        var chat = Chat.CreateDirect(_currentUser.UserId, command.RecipientId);

        await _unitOfWork.Chats.AddAsync(chat, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(chat.Id);
    }
}
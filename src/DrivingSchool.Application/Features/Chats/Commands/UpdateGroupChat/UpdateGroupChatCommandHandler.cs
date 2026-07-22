using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.UpdateGroupChat;

/// <summary>Handles <see cref="UpdateGroupChatCommand"/>.</summary>
internal sealed class UpdateGroupChatCommandHandler : ICommandHandler<UpdateGroupChatCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateGroupChatCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        UpdateGroupChatCommand command,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(command.ChatId, cancellationToken);
        if (chat is null)
            return Result.Failure(DomainErrors.Chat.NotFound);

        if (chat.Type == ChatType.Direct)
            return Result.Failure(DomainErrors.Chat.CannotRenameDirectChat);

        var requester = chat.Participants.FirstOrDefault(p => p.UserId == _currentUser.UserId);
        if (requester is null || requester.Role != ParticipantRole.Admin)
            return Result.Failure(DomainErrors.Chat.RequiresAdmin);

        chat.Rename(command.Name);

        _unitOfWork.Chats.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
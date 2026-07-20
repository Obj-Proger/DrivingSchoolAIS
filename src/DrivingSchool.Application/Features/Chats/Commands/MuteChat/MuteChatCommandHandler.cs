using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.MuteChat;

/// <summary>Handles <see cref="MuteChatCommand"/>.</summary>
internal sealed class MuteChatCommandHandler : ICommandHandler<MuteChatCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public MuteChatCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        MuteChatCommand command,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(command.ChatId, cancellationToken);
        if (chat is null)
            return Result.Failure(DomainErrors.Chat.NotFound);

        var participant = chat.Participants.FirstOrDefault(p => p.UserId == _currentUser.UserId);
        if (participant is null)
            return Result.Failure(DomainErrors.Chat.ParticipantNotFound);

        if (command.Mute) participant.Mute();
        else participant.Unmute();

        _unitOfWork.Chats.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
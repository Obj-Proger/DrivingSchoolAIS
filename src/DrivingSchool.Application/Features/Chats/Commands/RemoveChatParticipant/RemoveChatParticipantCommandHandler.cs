using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.RemoveChatParticipant;

/// <summary>Handles <see cref="RemoveChatParticipantCommand"/>.</summary>
internal sealed class RemoveChatParticipantCommandHandler
    : ICommandHandler<RemoveChatParticipantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public RemoveChatParticipantCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        RemoveChatParticipantCommand command,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(command.ChatId, cancellationToken);
        if (chat is null)
            return Result.Failure(DomainErrors.Chat.NotFound);

        // Leaving the chat yourself is always allowed; removing someone else
        // requires administrator privileges.
        var isSelfRemoval = command.UserId == _currentUser.UserId;
        if (!isSelfRemoval)
        {
            var requester = chat.Participants.FirstOrDefault(p => p.UserId == _currentUser.UserId);
            if (requester is null || requester.Role != ParticipantRole.Admin)
                return Result.Failure(DomainErrors.Chat.RequiresAdmin);
        }

        var result = chat.RemoveParticipant(command.UserId);
        if (result.IsFailure) return result;

        _unitOfWork.Chats.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
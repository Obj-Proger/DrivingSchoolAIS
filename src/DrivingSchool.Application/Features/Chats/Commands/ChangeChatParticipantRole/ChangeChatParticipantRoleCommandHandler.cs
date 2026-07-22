using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.ChangeChatParticipantRole;

/// <summary>Handles <see cref="ChangeChatParticipantRoleCommand"/>.</summary>
internal sealed class ChangeChatParticipantRoleCommandHandler
    : ICommandHandler<ChangeChatParticipantRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ChangeChatParticipantRoleCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        ChangeChatParticipantRoleCommand command,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(command.ChatId, cancellationToken);
        if (chat is null)
            return Result.Failure(DomainErrors.Chat.NotFound);

        var requester = chat.Participants.FirstOrDefault(p => p.UserId == _currentUser.UserId);
        if (requester is null || requester.Role != ParticipantRole.Admin)
            return Result.Failure(DomainErrors.Chat.RequiresAdmin);

        var result = chat.ChangeParticipantRole(command.UserId, command.NewRole);
        if (result.IsFailure) return result;

        _unitOfWork.Chats.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
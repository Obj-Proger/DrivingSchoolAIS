using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.AddChatParticipant;

/// <summary>Handles <see cref="AddChatParticipantCommand"/>.</summary>
internal sealed class AddChatParticipantCommandHandler
    : ICommandHandler<AddChatParticipantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AddChatParticipantCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        AddChatParticipantCommand command,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(command.ChatId, cancellationToken);
        if (chat is null)
            return Result.Failure(DomainErrors.Chat.NotFound);

        var requester = chat.Participants.FirstOrDefault(p => p.UserId == _currentUser.UserId);
        if (requester is null || requester.Role != ParticipantRole.Admin)
            return Result.Failure(DomainErrors.Chat.RequiresAdmin);

        var newUser = await _unitOfWork.Users.GetByIdAsync(command.UserId, cancellationToken);
        if (newUser is null)
            return Result.Failure(DomainErrors.User.NotFound);

        var result = chat.AddParticipant(command.UserId, command.Role);
        if (result.IsFailure) return result;

        _unitOfWork.Chats.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
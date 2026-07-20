using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.CreateGroupChat;

/// <summary>Handles <see cref="CreateGroupChatCommand"/>.</summary>
internal sealed class CreateGroupChatCommandHandler
    : ICommandHandler<CreateGroupChatCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateGroupChatCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateGroupChatCommand command,
        CancellationToken cancellationToken)
    {
        var chatResult = Chat.Create(
            command.Type,
            command.Name,
            _currentUser.UserId,
            command.ParticipantIds);

        if (chatResult.IsFailure)
            return Result.Failure<Guid>(chatResult.Error);

        await _unitOfWork.Chats.AddAsync(chatResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(chatResult.Value.Id);
    }
}
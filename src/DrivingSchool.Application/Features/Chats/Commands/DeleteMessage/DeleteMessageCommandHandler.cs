using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Commands.DeleteMessage;

/// <summary>Handles <see cref="DeleteMessageCommand"/>.</summary>
internal sealed class DeleteMessageCommandHandler : ICommandHandler<DeleteMessageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeleteMessageCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        DeleteMessageCommand command,
        CancellationToken cancellationToken)
    {
        var message = await _unitOfWork.Chats
            .GetMessageByIdAsync(command.MessageId, cancellationToken);

        if (message is null)
            return Result.Failure(DomainErrors.Message.NotFound);

        if (message.SenderId != _currentUser.UserId)
            return Result.Failure(DomainErrors.Message.CannotDeleteOthers);

        if (message.IsDeleted)
            return Result.Failure(DomainErrors.Message.AlreadyDeleted);

        message.SoftDelete();

        _unitOfWork.Chats.UpdateMessage(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
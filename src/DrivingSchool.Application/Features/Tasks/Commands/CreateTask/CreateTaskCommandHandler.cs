using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Tasks.Commands.CreateTask;

/// <summary>Handles <see cref="CreateTaskCommand"/>.</summary>
internal sealed class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateTaskCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var assignee = await _unitOfWork.Users
            .GetByIdAsync(command.AssignedToId, cancellationToken);

        if (assignee is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound);

        var task = StaffTask.Create(
            command.Title,
            command.Description,
            command.AssignedToId,
            _currentUser.UserId,
            command.Priority,
            command.DueDate,
            command.LinkedEntityType,
            command.LinkedEntityId);

        await _unitOfWork.StaffTasks.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(task.Id);
    }
}
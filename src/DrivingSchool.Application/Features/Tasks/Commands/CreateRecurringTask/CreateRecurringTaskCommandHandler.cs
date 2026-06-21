using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Tasks.Commands.CreateRecurringTask;

/// <summary>Handles <see cref="CreateRecurringTaskCommand"/>.</summary>
internal sealed class CreateRecurringTaskCommandHandler
    : ICommandHandler<CreateRecurringTaskCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateRecurringTaskCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateRecurringTaskCommand command,
        CancellationToken cancellationToken)
    {
        var assignee = await _unitOfWork.Users
            .GetByIdAsync(command.AssignedToId, cancellationToken);

        if (assignee is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound);

        var task = StaffTask.CreateRecurring(
            command.Title,
            command.Description,
            command.AssignedToId,
            _currentUser.UserId,
            command.Priority,
            command.RecurringDays);

        await _unitOfWork.StaffTasks.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(task.Id);
    }
}
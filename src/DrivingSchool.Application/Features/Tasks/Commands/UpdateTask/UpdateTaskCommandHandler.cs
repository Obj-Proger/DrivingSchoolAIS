using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Tasks.Commands.UpdateTask;

/// <summary>Handles <see cref="UpdateTaskCommand"/>.</summary>
internal sealed class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.StaffTasks
            .GetByIdAsync(command.TaskId, cancellationToken);

        if (task is null)
            return Result.Failure(DomainErrors.StaffTask.NotFound);

        task.Update(command.Title, command.Description);
        task.UpdatePriority(command.Priority);
        task.UpdateDueDate(command.DueDate);

        _unitOfWork.StaffTasks.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
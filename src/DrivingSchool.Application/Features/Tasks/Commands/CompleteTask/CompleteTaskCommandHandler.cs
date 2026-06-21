using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Tasks.Commands.CompleteTask;

/// <summary>Handles <see cref="CompleteTaskCommand"/>.</summary>
internal sealed class CompleteTaskCommandHandler : ICommandHandler<CompleteTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTaskCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CompleteTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.StaffTasks
            .GetByIdAsync(command.TaskId, cancellationToken);

        if (task is null) return Result.Failure(DomainErrors.StaffTask.NotFound);

        var result = task.Complete();
        if (result.IsFailure) return result;

        _unitOfWork.StaffTasks.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
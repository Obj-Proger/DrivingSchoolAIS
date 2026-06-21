using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Tasks.Commands.DeleteTask;

/// <summary>Handles <see cref="DeleteTaskCommand"/>.</summary>
internal sealed class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.StaffTasks
            .GetByIdAsync(command.TaskId, cancellationToken);

        if (task is null) return Result.Failure(DomainErrors.StaffTask.NotFound);

        _unitOfWork.StaffTasks.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
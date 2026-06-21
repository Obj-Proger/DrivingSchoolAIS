using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Tasks.Commands.ReopenTask;

/// <summary>Handles <see cref="ReopenTaskCommand"/>.</summary>
internal sealed class ReopenTaskCommandHandler : ICommandHandler<ReopenTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReopenTaskCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ReopenTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.StaffTasks
            .GetByIdAsync(command.TaskId, cancellationToken);

        if (task is null) return Result.Failure(DomainErrors.StaffTask.NotFound);

        task.Reopen();

        _unitOfWork.StaffTasks.Update(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
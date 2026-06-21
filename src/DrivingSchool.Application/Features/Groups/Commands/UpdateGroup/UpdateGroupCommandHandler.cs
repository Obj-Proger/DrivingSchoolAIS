using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Commands.UpdateGroup;

/// <summary>Handles <see cref="UpdateGroupCommand"/>.</summary>
internal sealed class UpdateGroupCommandHandler : ICommandHandler<UpdateGroupCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGroupCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateGroupCommand command,
        CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Groups
            .GetByIdAsync(command.GroupId, cancellationToken);

        if (group is null) return Result.Failure(DomainErrors.Group.NotFound);

        group.Update(command.Name, command.TeacherId, command.MaxStudents, command.EndDate);

        _unitOfWork.Groups.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Commands.ArchiveGroup;

/// <summary>Handles <see cref="ArchiveGroupCommand"/>.</summary>
internal sealed class ArchiveGroupCommandHandler : ICommandHandler<ArchiveGroupCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveGroupCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ArchiveGroupCommand command,
        CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Groups
            .GetByIdAsync(command.GroupId, cancellationToken);

        if (group is null) return Result.Failure(DomainErrors.Group.NotFound);

        group.Archive();

        _unitOfWork.Groups.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
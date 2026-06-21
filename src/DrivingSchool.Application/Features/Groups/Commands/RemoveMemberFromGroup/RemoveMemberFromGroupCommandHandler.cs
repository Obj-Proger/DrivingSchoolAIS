using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Commands.RemoveMemberFromGroup;

/// <summary>Handles <see cref="RemoveMemberFromGroupCommand"/>.</summary>
internal sealed class RemoveMemberFromGroupCommandHandler
    : ICommandHandler<RemoveMemberFromGroupCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMemberFromGroupCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        RemoveMemberFromGroupCommand command,
        CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Groups
            .GetByIdWithMembersAsync(command.GroupId, cancellationToken);

        if (group is null) return Result.Failure(DomainErrors.Group.NotFound);

        var result = group.RemoveMember(command.ContractId);
        if (result.IsFailure) return result;

        _unitOfWork.Groups.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
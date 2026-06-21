using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Commands.AddMemberToGroup;

/// <summary>Handles <see cref="AddMemberToGroupCommand"/>.</summary>
internal sealed class AddMemberToGroupCommandHandler
    : ICommandHandler<AddMemberToGroupCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddMemberToGroupCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        AddMemberToGroupCommand command,
        CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Groups
            .GetByIdWithMembersAsync(command.GroupId, cancellationToken);

        if (group is null) return Result.Failure(DomainErrors.Group.NotFound);

        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(command.ContractId, cancellationToken);

        if (contract is null) return Result.Failure(DomainErrors.Contract.NotFound);

        var addResult = group.AddMember(command.ContractId);
        if (addResult.IsFailure) return addResult;

        contract.AssignToGroup(command.GroupId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _unitOfWork.Groups.Update(group);
        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}
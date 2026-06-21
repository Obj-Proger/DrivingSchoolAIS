using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Contracts.Commands.AssignContractToGroup;

/// <summary>
/// Handles <see cref="AssignContractToGroupCommand"/>.
/// Validates group capacity and membership before assigning.
/// </summary>
internal sealed class AssignContractToGroupCommandHandler
    : ICommandHandler<AssignContractToGroupCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignContractToGroupCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        AssignContractToGroupCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Load entities
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(command.ContractId, cancellationToken);

        if (contract is null)
            return Result.Failure(DomainErrors.Contract.NotFound);

        if (contract.Status != ContractStatus.Active)
            return Result.Failure(DomainErrors.Contract.AlreadyClosed);

        var group = await _unitOfWork.Groups
            .GetByIdWithMembersAsync(command.GroupId, cancellationToken);

        if (group is null)
            return Result.Failure(DomainErrors.Group.NotFound);

        // 2. Add member to group aggregate
        var addResult = group.AddMember(command.ContractId);
        if (addResult.IsFailure) return addResult;

        // 3. Link group to contract
        contract.AssignToGroup(command.GroupId);

        // 4. Persist atomically
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        _unitOfWork.Groups.Update(group);
        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}
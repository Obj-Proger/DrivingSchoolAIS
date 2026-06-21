using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Contracts.Commands.TerminateContract;

/// <summary>
/// Handles <see cref="TerminateContractCommand"/>.
/// </summary>
internal sealed class TerminateContractCommandHandler : ICommandHandler<TerminateContractCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public TerminateContractCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        TerminateContractCommand command,
        CancellationToken cancellationToken)
    {
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(command.ContractId, cancellationToken);

        if (contract is null)
            return Result.Failure(DomainErrors.Contract.NotFound);

        var result = contract.Terminate(command.Reason);
        if (result.IsFailure) return result;

        _unitOfWork.Contracts.Update(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Branches.Commands.DeactivateBranch;

/// <summary>Handles <see cref="DeactivateBranchCommand"/>.</summary>
internal sealed class DeactivateBranchCommandHandler : ICommandHandler<DeactivateBranchCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateBranchCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeactivateBranchCommand command,
        CancellationToken cancellationToken)
    {
        var branch = await _unitOfWork.Branches
            .GetByIdAsync(command.BranchId, cancellationToken);

        if (branch is null)
            return Result.Failure(DomainErrors.Branch.NotFound);

        var result = branch.Deactivate();
        if (result.IsFailure) return result;

        _unitOfWork.Branches.Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
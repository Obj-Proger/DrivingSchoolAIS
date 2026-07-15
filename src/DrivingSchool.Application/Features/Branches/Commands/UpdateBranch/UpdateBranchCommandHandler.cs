using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Branches.Commands.UpdateBranch;

/// <summary>Handles <see cref="UpdateBranchCommand"/>.</summary>
internal sealed class UpdateBranchCommandHandler : ICommandHandler<UpdateBranchCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBranchCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateBranchCommand command,
        CancellationToken cancellationToken)
    {
        var branch = await _unitOfWork.Branches
            .GetByIdAsync(command.BranchId, cancellationToken);

        if (branch is null)
            return Result.Failure(DomainErrors.Branch.NotFound);

        var phoneResult = PhoneNumber.Create(command.Phone);
        if (phoneResult.IsFailure)
            return Result.Failure(phoneResult.Error);

        var updateResult = branch.Update(
            command.Name,
            command.City,
            command.Address,
            phoneResult.Value);

        if (updateResult.IsFailure)
            return updateResult;

        _unitOfWork.Branches.Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
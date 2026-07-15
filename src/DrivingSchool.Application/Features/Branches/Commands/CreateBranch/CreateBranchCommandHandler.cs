using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Branches.Commands.CreateBranch;

/// <summary>Handles <see cref="CreateBranchCommand"/>.</summary>
internal sealed class CreateBranchCommandHandler : ICommandHandler<CreateBranchCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBranchCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateBranchCommand command,
        CancellationToken cancellationToken)
    {
        var phoneResult = PhoneNumber.Create(command.Phone);
        if (phoneResult.IsFailure)
            return Result.Failure<Guid>(phoneResult.Error);

        var branchResult = Branch.Create(
            command.Name,
            command.City,
            command.Address,
            phoneResult.Value);

        if (branchResult.IsFailure)
            return Result.Failure<Guid>(branchResult.Error);

        await _unitOfWork.Branches.AddAsync(branchResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(branchResult.Value.Id);
    }
}
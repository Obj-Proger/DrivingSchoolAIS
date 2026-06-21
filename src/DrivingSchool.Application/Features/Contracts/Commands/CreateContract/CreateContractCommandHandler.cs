using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Contracts.Commands.CreateContract;

/// <summary>
/// Handles <see cref="CreateContractCommand"/>.
/// Validates uniqueness of the contract number and creates the contract aggregate.
/// </summary>
internal sealed class CreateContractCommandHandler
    : ICommandHandler<CreateContractCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateContractCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateContractCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Validate student exists
        var student = await _unitOfWork.Users
            .GetByIdAsync(command.StudentId, cancellationToken);

        if (student is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound);

        // 2. Validate contract number uniqueness
        var numberExists = await _unitOfWork.Contracts
            .ExistsByNumberAsync(command.Number, cancellationToken);

        if (numberExists)
            return Result.Failure<Guid>(Error.Conflict("Contract"));

        // 3. Build money value object
        var moneyResult = Money.Create(command.TotalCost);
        if (moneyResult.IsFailure)
            return Result.Failure<Guid>(moneyResult.Error);

        // 4. Create contract
        var contractResult = Contract.Create(
            command.Number,
            command.StudentId,
            command.CourseId,
            command.StartDate,
            command.EndDate,
            moneyResult.Value);

        if (contractResult.IsFailure)
            return Result.Failure<Guid>(contractResult.Error);

        // 5. Persist
        await _unitOfWork.Contracts.AddAsync(contractResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(contractResult.Value.Id);
    }
}
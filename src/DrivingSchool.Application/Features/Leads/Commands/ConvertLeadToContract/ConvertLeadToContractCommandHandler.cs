using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.ConvertLeadToContract;

/// <summary>
/// Handles <see cref="ConvertLeadToContractCommand"/>.
/// Creates a Contract from the lead's data, marks the lead as converted,
/// and links the student's user account.
/// </summary>
internal sealed class ConvertLeadToContractCommandHandler
    : ICommandHandler<ConvertLeadToContractCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConvertLeadToContractCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        ConvertLeadToContractCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Load and validate lead
        var lead = await _unitOfWork.Leads.GetByIdAsync(command.LeadId, cancellationToken);
        if (lead is null)
            return Result.Failure<Guid>(DomainErrors.Lead.NotFound);

        // 2. Validate course
        var numberExists = await _unitOfWork.Contracts
            .ExistsByNumberAsync(command.ContractNumber, cancellationToken);
        if (numberExists)
            return Result.Failure<Guid>(Error.Conflict("Contract"));

        // 3. Find or create student user account
        // The student account must exist; leads are converted for users
        // who have registered or been manually added to the system.
        var studentUser = await _unitOfWork.Users
            .GetByEmailAsync(lead.Email?.Value ?? string.Empty, cancellationToken);

        if (studentUser is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound);

        // 4. Create contract
        var priceResult = Money.Create(command.TotalCost);
        if (priceResult.IsFailure)
            return Result.Failure<Guid>(priceResult.Error);

        var contractResult = Contract.Create(
            command.ContractNumber,
            studentUser.Id,
            command.CourseId,
            command.StartDate,
            command.EndDate,
            priceResult.Value);

        if (contractResult.IsFailure)
            return Result.Failure<Guid>(contractResult.Error);

        var contract = contractResult.Value;

        // 5. Mark lead as converted
        var convertResult = lead.ConvertToContract(contract.Id);
        if (convertResult.IsFailure)
            return Result.Failure<Guid>(convertResult.Error);

        // 6. Persist atomically
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        await _unitOfWork.Contracts.AddAsync(contract, cancellationToken);
        _unitOfWork.Leads.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success(contract.Id);
    }
}
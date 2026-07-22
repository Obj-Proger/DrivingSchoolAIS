using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Payments.Commands.CreatePayment;

/// <summary>Handles <see cref="CreatePaymentCommand"/>.</summary>
internal sealed class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreatePaymentCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreatePaymentCommand command,
        CancellationToken cancellationToken)
    {
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(command.ContractId, cancellationToken);

        if (contract is null)
            return Result.Failure<Guid>(DomainErrors.Contract.NotFound);

        var amountResult = Money.Create(command.Amount);
        if (amountResult.IsFailure)
            return Result.Failure<Guid>(amountResult.Error);

        var payment = Payment.Create(
            command.ContractId,
            _currentUser.UserId,
            amountResult.Value,
            command.Purpose,
            command.Method);

        await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(payment.Id);
    }
}
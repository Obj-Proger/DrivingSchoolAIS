using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Payments.Commands.ConfirmPayment;

/// <summary>Handles <see cref="ConfirmPaymentCommand"/>.</summary>
internal sealed class ConfirmPaymentCommandHandler : ICommandHandler<ConfirmPaymentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmPaymentCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ConfirmPaymentCommand command,
        CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments
            .GetByIdAsync(command.PaymentId, cancellationToken);

        if (payment is null)
            return Result.Failure(DomainErrors.Payment.NotFound);

        var result = payment.Confirm(command.ReceiptNumber);
        if (result.IsFailure) return result;

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
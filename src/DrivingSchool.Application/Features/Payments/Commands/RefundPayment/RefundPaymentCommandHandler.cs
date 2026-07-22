using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Payments.Commands.RefundPayment;

/// <summary>Handles <see cref="RefundPaymentCommand"/>.</summary>
internal sealed class RefundPaymentCommandHandler : ICommandHandler<RefundPaymentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RefundPaymentCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        RefundPaymentCommand command,
        CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments
            .GetByIdAsync(command.PaymentId, cancellationToken);

        if (payment is null)
            return Result.Failure(DomainErrors.Payment.NotFound);

        var result = payment.Refund();
        if (result.IsFailure) return result;

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
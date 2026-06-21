using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.UpdateLeadStatus;

/// <summary>Handles <see cref="UpdateLeadStatusCommand"/>.</summary>
internal sealed class UpdateLeadStatusCommandHandler : ICommandHandler<UpdateLeadStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLeadStatusCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateLeadStatusCommand command,
        CancellationToken cancellationToken)
    {
        var lead = await _unitOfWork.Leads.GetByIdAsync(command.LeadId, cancellationToken);
        if (lead is null) return Result.Failure(DomainErrors.Lead.NotFound);

        var result = lead.UpdateStatus(command.NewStatus);
        if (result.IsFailure) return result;

        _unitOfWork.Leads.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
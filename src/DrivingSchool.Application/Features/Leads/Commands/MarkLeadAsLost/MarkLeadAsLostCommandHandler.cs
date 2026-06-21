using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.MarkLeadAsLost;

/// <summary>Handles <see cref="MarkLeadAsLostCommand"/>.</summary>
internal sealed class MarkLeadAsLostCommandHandler : ICommandHandler<MarkLeadAsLostCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkLeadAsLostCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        MarkLeadAsLostCommand command,
        CancellationToken cancellationToken)
    {
        var lead = await _unitOfWork.Leads.GetByIdAsync(command.LeadId, cancellationToken);
        if (lead is null) return Result.Failure(DomainErrors.Lead.NotFound);

        var result = lead.MarkAsLost(command.Reason, command.AuthorId);
        if (result.IsFailure) return result;

        _unitOfWork.Leads.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
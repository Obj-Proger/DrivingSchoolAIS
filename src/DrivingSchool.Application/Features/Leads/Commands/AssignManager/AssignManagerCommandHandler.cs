using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.AssignManager;

/// <summary>Handles <see cref="AssignManagerCommand"/>.</summary>
internal sealed class AssignManagerCommandHandler : ICommandHandler<AssignManagerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignManagerCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        AssignManagerCommand command,
        CancellationToken cancellationToken)
    {
        var lead = await _unitOfWork.Leads.GetByIdAsync(command.LeadId, cancellationToken);
        if (lead is null) return Result.Failure(DomainErrors.Lead.NotFound);

        var manager = await _unitOfWork.Users.GetByIdAsync(command.ManagerId, cancellationToken);
        if (manager is null) return Result.Failure(DomainErrors.User.NotFound);

        lead.AssignManager(command.ManagerId);

        _unitOfWork.Leads.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
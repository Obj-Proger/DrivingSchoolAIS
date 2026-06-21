using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.UpdateLead;

/// <summary>Handles <see cref="UpdateLeadCommand"/>.</summary>
internal sealed class UpdateLeadCommandHandler : ICommandHandler<UpdateLeadCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLeadCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateLeadCommand command,
        CancellationToken cancellationToken)
    {
        var lead = await _unitOfWork.Leads.GetByIdAsync(command.LeadId, cancellationToken);
        if (lead is null) return Result.Failure(DomainErrors.Lead.NotFound);

        var fullNameResult = FullName.Create(command.FirstName, command.LastName, command.MiddleName);
        if (fullNameResult.IsFailure) return Result.Failure(fullNameResult.Error);

        var phoneResult = PhoneNumber.Create(command.Phone);
        if (phoneResult.IsFailure) return Result.Failure(phoneResult.Error);

        Email? email = null;
        if (command.Email is not null)
        {
            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure) return Result.Failure(emailResult.Error);
            email = emailResult.Value;
        }

        lead.Update(fullNameResult.Value, phoneResult.Value, email,
            command.CourseInterest, command.Comment);

        _unitOfWork.Leads.Update(lead);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
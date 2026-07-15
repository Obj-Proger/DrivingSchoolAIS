using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Commands.CreateLead;

/// <summary>
/// Handles <see cref="CreateLeadCommand"/>.
/// </summary>
internal sealed class CreateLeadCommandHandler : ICommandHandler<CreateLeadCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateLeadCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateLeadCommand command,
        CancellationToken cancellationToken)
    {
        var fullNameResult = FullName.Create(
            command.FirstName, command.LastName, command.MiddleName);
        if (fullNameResult.IsFailure)
            return Result.Failure<Guid>(fullNameResult.Error);

        var phoneResult = PhoneNumber.Create(command.Phone);
        if (phoneResult.IsFailure)
            return Result.Failure<Guid>(phoneResult.Error);

        Email? email = null;
        if (command.Email is not null)
        {
            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure)
                return Result.Failure<Guid>(emailResult.Error);
            email = emailResult.Value;
        }

        var lead = Lead.Create(
            fullNameResult.Value,
            phoneResult.Value,
            command.Source,
            email,
            command.CourseInterest,
            command.Comment,
            command.ResponsibleManagerId,
            command.BranchId);

        await _unitOfWork.Leads.AddAsync(lead, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(lead.Id);
    }
}
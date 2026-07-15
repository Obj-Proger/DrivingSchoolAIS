using DrivingSchool.Application.Features.Groups.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Queries.GetGroupById;

/// <summary>
/// Handles <see cref="GetGroupByIdQuery"/>.
/// </summary>
internal sealed class GetGroupByIdQueryHandler
    : IQueryHandler<GetGroupByIdQuery, GroupDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGroupByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<GroupDetailDto>> Handle(
        GetGroupByIdQuery query,
        CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Groups
            .GetByIdWithMembersAsync(query.GroupId, cancellationToken);

        if (group is null)
            return Result.Failure<GroupDetailDto>(DomainErrors.Group.NotFound);

        var teacher = await _unitOfWork.Users
            .GetByIdAsync(group.TeacherId, cancellationToken);

        // Build member DTOs
        var memberDtos = new List<GroupMemberDto>();
        foreach (var member in group.Members)
        {
            var contract = await _unitOfWork.Contracts
                .GetByIdAsync(member.ContractId, cancellationToken);

            if (contract is null) continue;

            var student = await _unitOfWork.Users
                .GetByIdAsync(contract.StudentId, cancellationToken);

            memberDtos.Add(new GroupMemberDto(
                member.ContractId,
                contract.StudentId,
                student?.FullName.DisplayName ?? "Unknown",
                student?.Phone.Value ?? string.Empty,
                contract.PracticeHoursCompleted,
                contract.TheoryLessonsAttended,
                contract.DebtAmount.Amount,
                contract.QualityIndicator,
                member.JoinedAt));
        }

        return Result.Success(new GroupDetailDto(
            group.Id,
            group.Name,
            group.CourseId,
            string.Empty,
            LicenseCategory.B,
            group.TeacherId,
            teacher?.FullName.ShortName ?? "Unknown",
            group.Status,
            group.MaxStudents,
            group.StartDate,
            group.EndDate,
            group.BranchId,
            memberDtos));
    }
}
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Groups.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Queries.GetGroups;

/// <summary>
/// Handles <see cref="GetGroupsQuery"/>.
/// </summary>
internal sealed class GetGroupsQueryHandler
    : IQueryHandler<GetGroupsQuery, PaginatedResult<GroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGroupsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<GroupDto>>> Handle(
        GetGroupsQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Groups.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            query.Status,
            query.TeacherId,
            cancellationToken);

        var dtos = new List<GroupDto>();
        foreach (var group in paginated.Items)
        {
            var dto = await MapToDtoAsync(group, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success(new PaginatedResult<GroupDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }

    internal async Task<GroupDto> MapToDtoAsync(Group group, CancellationToken ct)
    {
        var teacher = await _unitOfWork.Users.GetByIdAsync(group.TeacherId, ct);
        var course = await _unitOfWork.Courses.GetByIdAsync(group.CourseId, ct);

        return new GroupDto(
            group.Id,
            group.Name,
            group.CourseId,
            course?.Name ?? "Unknown",
            course?.Category ?? LicenseCategory.B,
            group.TeacherId,
            teacher?.FullName.ShortName ?? "Unknown",
            group.Status,
            group.Members.Count,
            group.MaxStudents,
            group.StartDate,
            group.EndDate,
            group.BranchId);
    }
}
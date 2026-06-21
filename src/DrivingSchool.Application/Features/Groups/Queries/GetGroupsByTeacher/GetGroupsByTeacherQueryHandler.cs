using DrivingSchool.Application.Features.Groups.DTOs;
using DrivingSchool.Application.Features.Groups.Queries.GetGroups;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Queries.GetGroupsByTeacher;

/// <summary>
/// Handles <see cref="GetGroupsByTeacherQuery"/>.
/// </summary>
internal sealed class GetGroupsByTeacherQueryHandler
    : IQueryHandler<GetGroupsByTeacherQuery, IReadOnlyList<GroupDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetGroupsQueryHandler _mapper;

    public GetGroupsByTeacherQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new GetGroupsQueryHandler(unitOfWork);
    }

    public async Task<Result<IReadOnlyList<GroupDto>>> Handle(
        GetGroupsByTeacherQuery query,
        CancellationToken cancellationToken)
    {
        var groups = await _unitOfWork.Groups
            .GetByTeacherIdAsync(query.TeacherId, cancellationToken);

        var dtos = new List<GroupDto>();
        foreach (var group in groups)
        {
            var dto = await _mapper.MapToDtoAsync(group, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success<IReadOnlyList<GroupDto>>(dtos);
    }
}
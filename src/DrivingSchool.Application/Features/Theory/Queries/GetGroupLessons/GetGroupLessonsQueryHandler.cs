using DrivingSchool.Application.Features.Theory.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Queries.GetGroupLessons;

/// <summary>
/// Handles <see cref="GetGroupLessonsQuery"/>.
/// </summary>
internal sealed class GetGroupLessonsQueryHandler
    : IQueryHandler<GetGroupLessonsQuery, IReadOnlyList<LessonDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGroupLessonsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<LessonDto>>> Handle(
        GetGroupLessonsQuery query,
        CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.Groups
            .GetByIdAsync(query.GroupId, cancellationToken);

        if (group is null)
            return Result.Failure<IReadOnlyList<LessonDto>>(DomainErrors.Group.NotFound);

        var lessons = await _unitOfWork.TheoryLessons.GetByGroupIdAsync(
            query.GroupId, query.From, query.To, cancellationToken);

        var dtos = new List<LessonDto>();

        foreach (var lesson in lessons)
        {
            var dto = await MapToLessonDtoAsync(lesson, group.Name, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success<IReadOnlyList<LessonDto>>(dtos);
    }

    internal async Task<LessonDto> MapToLessonDtoAsync(
        TheoryLesson lesson,
        string groupName,
        CancellationToken ct)
    {
        var teacher = await _unitOfWork.Users.GetByIdAsync(lesson.TeacherId, ct);

        return new LessonDto(
            lesson.Id,
            lesson.GroupId,
            groupName,
            lesson.TeacherId,
            teacher?.FullName.ShortName ?? "Unknown",
            lesson.TopicId,
            string.Empty, // topic title resolved in Infrastructure
            lesson.Title,
            lesson.Description,
            lesson.ScheduledAt,
            lesson.DurationMinutes,
            lesson.RoomOrLink,
            lesson.Status,
            lesson.CancellationReason,
            lesson.Materials.Count,
            lesson.AttendanceRecords.Count,
            lesson.CreatedAt);
    }
}
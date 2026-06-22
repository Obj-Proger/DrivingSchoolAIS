using DrivingSchool.Application.Features.Theory.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Queries.GetLessonById;

/// <summary>
/// Handles <see cref="GetLessonByIdQuery"/>.
/// </summary>
internal sealed class GetLessonByIdQueryHandler
    : IQueryHandler<GetLessonByIdQuery, LessonDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLessonByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<LessonDetailDto>> Handle(
        GetLessonByIdQuery query,
        CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.TheoryLessons
            .GetByIdWithDetailsAsync(query.LessonId, cancellationToken);

        if (lesson is null)
            return Result.Failure<LessonDetailDto>(DomainErrors.TheoryLesson.NotFound);

        var group = await _unitOfWork.Groups
            .GetByIdAsync(lesson.GroupId, cancellationToken);

        var teacher = await _unitOfWork.Users
            .GetByIdAsync(lesson.TeacherId, cancellationToken);

        // Materials
        var materialDtos = lesson.Materials
            .Select(m => new LessonMaterialDto(
                m.Id,
                m.LessonId,
                m.TopicId,
                m.Title,
                m.FileUrl,
                m.ContentType,
                m.SizeBytes,
                m.IsPublic,
                m.CreatedAt))
            .ToList();

        // Attendance
        var attendanceDtos = new List<AttendanceDto>();

        foreach (var record in lesson.AttendanceRecords)
        {
            var contract = await _unitOfWork.Contracts
                .GetByIdAsync(record.ContractId, cancellationToken);

            if (contract is null) continue;

            var student = await _unitOfWork.Users
                .GetByIdAsync(contract.StudentId, cancellationToken);

            attendanceDtos.Add(new AttendanceDto(
                record.ContractId,
                contract.StudentId,
                student?.FullName.DisplayName ?? "Unknown",
                record.IsPresent,
                record.Note));
        }

        return Result.Success(new LessonDetailDto(
            lesson.Id,
            lesson.GroupId,
            group?.Name ?? "Unknown",
            lesson.TeacherId,
            teacher?.FullName.ShortName ?? "Unknown",
            lesson.TopicId,
            string.Empty,
            lesson.Title,
            lesson.Description,
            lesson.ScheduledAt,
            lesson.DurationMinutes,
            lesson.RoomOrLink,
            lesson.Status,
            lesson.CancellationReason,
            materialDtos,
            attendanceDtos,
            lesson.CreatedAt));
    }
}
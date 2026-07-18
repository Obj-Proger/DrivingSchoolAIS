using DrivingSchool.Application.Features.Courses.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Queries.GetCourseById;

/// <summary>Handles <see cref="GetCourseByIdQuery"/>.</summary>
internal sealed class GetCourseByIdQueryHandler
    : IQueryHandler<GetCourseByIdQuery, CourseDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<CourseDetailDto>> Handle(
        GetCourseByIdQuery query,
        CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses
            .GetByIdWithTopicsAsync(query.CourseId, cancellationToken);

        if (course is null)
            return Result.Failure<CourseDetailDto>(DomainErrors.Course.NotFound);

        var topicDtos = course.Topics
            .OrderBy(t => t.OrderIndex)
            .Select(t => new CourseTopicDto(
                t.Id,
                t.CourseId,
                t.OrderIndex,
                t.Title,
                t.Description))
            .ToList();

        return Result.Success(new CourseDetailDto(
            course.Id,
            course.Name,
            course.Description,
            course.Category,
            course.TheoryHoursTotal,
            course.PracticeHoursTotal,
            course.Price.Amount,
            course.IsActive,
            topicDtos));
    }
}
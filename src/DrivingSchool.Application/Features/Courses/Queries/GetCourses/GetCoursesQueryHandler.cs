using DrivingSchool.Application.Features.Courses.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Queries.GetCourses;

/// <summary>Handles <see cref="GetCoursesQuery"/>.</summary>
internal sealed class GetCoursesQueryHandler
    : IQueryHandler<GetCoursesQuery, IReadOnlyList<CourseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCoursesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<CourseDto>>> Handle(
        GetCoursesQuery query,
        CancellationToken cancellationToken)
    {
        // Courses are a small, low-volume catalog table, so the Infrastructure
        // implementation of GetAllAsync is expected to eagerly load Topics
        // (unlike larger aggregates such as Lead or Group).
        var courses = await _unitOfWork.Courses
            .GetAllAsync(query.ActiveOnly, cancellationToken);

        var dtos = courses
            .Select(c => new CourseDto(
                c.Id,
                c.Name,
                c.Description,
                c.Category,
                c.TheoryHoursTotal,
                c.PracticeHoursTotal,
                c.Price.Amount,
                c.IsActive,
                c.Topics.Count))
            .OrderBy(c => c.Name)
            .ToList();

        return Result.Success<IReadOnlyList<CourseDto>>(dtos);
    }
}
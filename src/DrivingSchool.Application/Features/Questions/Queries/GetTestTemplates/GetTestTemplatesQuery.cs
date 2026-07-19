using DrivingSchool.Application.Features.Questions.DTOs;

namespace DrivingSchool.Application.Features.Questions.Queries.GetTestTemplates;

/// <summary>Returns all test templates, used to populate template selection lists.</summary>
public sealed record GetTestTemplatesQuery(
    bool ActiveOnly = true,
    Guid? CourseId = null) : IQuery<IReadOnlyList<TestTemplateDto>>;
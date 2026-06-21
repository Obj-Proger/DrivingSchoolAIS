using DrivingSchool.Application.Features.Users.DTOs;

namespace DrivingSchool.Application.Features.Users.Queries.GetStudentCard;

/// <summary>
/// Returns the instructor-facing student card for the specified user.
/// Contains training progress, skill assessments, and financial overview.
/// </summary>
/// <param name="StudentId">The user identifier of the student.</param>
public sealed record GetStudentCardQuery(Guid StudentId) : IQuery<StudentCardDto>;
using DrivingSchool.Application.Features.Users.DTOs;

namespace DrivingSchool.Application.Features.Users.Queries.GetInstructors;

/// <summary>
/// Returns a flat list of all active instructors.
/// Used to populate instructor filter dropdowns in scheduling views.
/// </summary>
public sealed record GetInstructorsQuery : IQuery<IReadOnlyList<UserDto>>;
using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Groups.Commands.UpdateGroup;

/// <summary>Updates the configuration of an existing group.</summary>
public sealed record UpdateGroupCommand(
    Guid GroupId,
    string Name,
    Guid TeacherId,
    int MaxStudents,
    DateTime? EndDate) : ICommand;
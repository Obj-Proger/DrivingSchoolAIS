using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Groups.Commands.ArchiveGroup;

/// <summary>Archives a group, marking it as inactive.</summary>
public sealed record ArchiveGroupCommand(Guid GroupId) : ICommand;
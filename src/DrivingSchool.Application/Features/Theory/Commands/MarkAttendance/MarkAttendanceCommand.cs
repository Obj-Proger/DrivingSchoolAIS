using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Theory.DTOs;

namespace DrivingSchool.Application.Features.Theory.Commands.MarkAttendance;

/// <summary>
/// Records or updates the attendance of students at a theory lesson.
/// Accepts a batch of attendance records in a single request.
/// </summary>
public sealed record MarkAttendanceCommand(
    Guid LessonId,
    IReadOnlyList<AttendanceRecordRequest> Records) : ICommand;
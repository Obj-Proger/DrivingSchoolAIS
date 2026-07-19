namespace DrivingSchool.Application.Features.Questions.DTOs;

/// <summary>Represents a single answer option shown for a question.</summary>
public sealed record QuestionOptionDto(Guid Id, string Text);
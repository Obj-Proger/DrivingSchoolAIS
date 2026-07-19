using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Questions.Commands.ImportQuestions;

/// <summary>
/// Bulk-imports questions into the test bank from a JSON payload, following the
/// open ПДД question format (see e.g. github.com/etspring/pdd_russia): an array of
/// objects with <c>question</c>, <c>image</c>, <c>ticket_category</c> (comma-separated
/// licence categories), <c>answers</c> (<c>answer_text</c> / <c>is_correct</c>),
/// <c>answer_tip</c>, and <c>topic</c> (array of topic names).
/// A source question covering multiple categories is imported as one <see cref="Question"/>
/// per category. Topics are matched by title within the target course, and created
/// automatically if no matching topic exists yet.
/// Restricted to administrators.
/// </summary>
public sealed record ImportQuestionsCommand(
    Guid CourseId,
    string QuestionsJson) : ICommand<ImportQuestionsResultDto>;

/// <summary>Summary of a bulk import operation.</summary>
public sealed record ImportQuestionsResultDto(
    int Imported,
    int Skipped,
    IReadOnlyList<string> Errors);
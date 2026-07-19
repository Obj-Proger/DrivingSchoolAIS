using System.Text.Json;
using System.Text.Json.Serialization;
using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Questions.Commands.ImportQuestions;

/// <summary>Handles <see cref="ImportQuestionsCommand"/>.</summary>
internal sealed class ImportQuestionsCommandHandler
    : ICommandHandler<ImportQuestionsCommand, ImportQuestionsResultDto>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IUnitOfWork _unitOfWork;

    public ImportQuestionsCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<ImportQuestionsResultDto>> Handle(
        ImportQuestionsCommand command,
        CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses
            .GetByIdWithTopicsAsync(command.CourseId, cancellationToken);

        if (course is null)
            return Result.Failure<ImportQuestionsResultDto>(DomainErrors.Course.NotFound);

        List<ImportSourceItem>? items;
        try
        {
            items = JsonSerializer.Deserialize<List<ImportSourceItem>>(
                command.QuestionsJson, JsonOptions);
        }
        catch (JsonException ex)
        {
            return Result.Failure<ImportQuestionsResultDto>(
                new Error("Question.InvalidImportPayload", $"Malformed JSON payload: {ex.Message}"));
        }

        if (items is null or [])
            return Result.Success(new ImportQuestionsResultDto(0, 0, []));

        var questionsToAdd = new List<Question>();
        var errors = new List<string>();
        var skipped = 0;

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Question) ||
                item.Answers is null or { Count: < 2 })
            {
                skipped++;
                errors.Add($"Skipped \"{item.Title ?? item.Question ?? "(untitled)"}\": " +
                           "missing question text or fewer than two answers.");
                continue;
            }

            var correctIndex = item.Answers.FindIndex(a => a.IsCorrect);
            if (correctIndex < 0)
            {
                skipped++;
                errors.Add($"Skipped \"{item.Title ?? item.Question}\": no answer marked correct.");
                continue;
            }

            var topicTitle = item.Topic?.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t))
                              ?? "Общие вопросы";
            var topic = FindOrCreateTopic(course, topicTitle);

            var optionTexts = item.Answers.Select(a => a.AnswerText ?? string.Empty).ToList();

            var categories = ParseCategories(item.TicketCategory);
            if (categories.Count == 0)
            {
                skipped++;
                errors.Add($"Skipped \"{item.Title ?? item.Question}\": " +
                           $"no recognised licence category in \"{item.TicketCategory}\".");
                continue;
            }

            foreach (var category in categories)
            {
                var questionResult = Question.Create(
                    topic.Id,
                    category,
                    item.Question,
                    optionTexts,
                    correctIndex,
                    QuestionSource.Official,
                    item.Image,
                    item.AnswerTip);

                if (questionResult.IsFailure)
                {
                    skipped++;
                    errors.Add($"Skipped \"{item.Title ?? item.Question}\" ({category}): " +
                               $"{questionResult.Error.Message}");
                    continue;
                }

                questionsToAdd.Add(questionResult.Value);
            }
        }

        if (questionsToAdd.Count > 0)
            await _unitOfWork.Questions.AddRangeAsync(questionsToAdd, cancellationToken);

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new ImportQuestionsResultDto(
            questionsToAdd.Count, skipped, errors));
    }

    /// <summary>
    /// Finds an existing topic by title (case-insensitive) within the course,
    /// or creates and appends a new one if no match is found.
    /// </summary>
    private static CourseTopic FindOrCreateTopic(Course course, string title)
    {
        var existing = course.Topics.FirstOrDefault(
            t => string.Equals(t.Title, title, StringComparison.OrdinalIgnoreCase));

        if (existing is not null)
            return existing;

        var topic = CourseTopic.Create(course.Id, title);
        course.AddTopic(topic);
        return topic;
    }

    /// <summary>
    /// Parses a comma-separated licence category string (e.g. "A,A1,B,B1,M")
    /// into recognised <see cref="LicenseCategory"/> values, silently skipping
    /// any token that does not match a known category.
    /// </summary>
    private static List<LicenseCategory> ParseCategories(string? ticketCategory)
    {
        if (string.IsNullOrWhiteSpace(ticketCategory))
            return [];

        return ticketCategory
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(token => Enum.TryParse<LicenseCategory>(token, ignoreCase: true, out var category)
                ? category
                : (LicenseCategory?)null)
            .Where(c => c.HasValue)
            .Select(c => c!.Value)
            .Distinct()
            .ToList();
    }

    // Source JSON shape (see e.g. github.com/etspring/pdd_russia)

    private sealed class ImportSourceItem
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("ticket_category")]
        public string? TicketCategory { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("question")]
        public string? Question { get; set; }

        [JsonPropertyName("answers")]
        public List<ImportSourceAnswer>? Answers { get; set; }

        [JsonPropertyName("answer_tip")]
        public string? AnswerTip { get; set; }

        [JsonPropertyName("topic")]
        public List<string>? Topic { get; set; }
    }

    private sealed class ImportSourceAnswer
    {
        [JsonPropertyName("answer_text")]
        public string? AnswerText { get; set; }

        [JsonPropertyName("is_correct")]
        public bool IsCorrect { get; set; }
    }
}
namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for standalone <see cref="LessonMaterial"/> records
/// that are attached to a course topic rather than a specific lesson
/// (see <see cref="LessonMaterial.TopicId"/>). Lesson-attached materials are managed
/// as part of the <see cref="TheoryLesson"/> aggregate through <see cref="ITheoryLessonRepository"/>
/// and are not exposed here.
/// </summary>
public interface ILessonMaterialRepository
{
    /// <summary>Returns the topic-level material with the specified identifier, or <c>null</c> if not found.</summary>
    Task<LessonMaterial?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Returns all public materials attached directly to the specified course topic.</summary>
    Task<IReadOnlyList<LessonMaterial>> GetByTopicIdAsync(Guid topicId, CancellationToken ct = default);

    /// <summary>Adds a new topic-level material to the repository.</summary>
    Task AddAsync(LessonMaterial material, CancellationToken ct = default);

    /// <summary>Removes a topic-level material from the repository.</summary>
    void Delete(LessonMaterial material);
}
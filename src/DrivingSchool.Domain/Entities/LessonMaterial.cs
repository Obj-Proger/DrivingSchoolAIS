using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a file or resource attached to a theory lesson or course topic.
/// A material must be associated with either a lesson or a topic — not both.
/// Topic-level materials are available to all students studying that topic,
/// while lesson-level materials are specific to a particular lesson delivery.
/// </summary>
public sealed class LessonMaterial : BaseEntity
{
    private LessonMaterial() { } // Required by EF Core

    /// <summary>
    /// Gets the identifier of the lesson this material belongs to,
    /// or <c>null</c> if the material is attached to a topic.
    /// </summary>
    public Guid? LessonId { get; private set; }

    /// <summary>
    /// Gets the identifier of the course topic this material belongs to,
    /// or <c>null</c> if the material is attached to a specific lesson.
    /// </summary>
    public Guid? TopicId { get; private set; }

    /// <summary>Gets the display name of the material.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets the URL where the file can be accessed or downloaded.</summary>
    public string FileUrl { get; private set; } = string.Empty;

    /// <summary>Gets the MIME content type of the file (e.g. <c>application/pdf</c>).</summary>
    public string ContentType { get; private set; } = string.Empty;

    /// <summary>Gets the size of the file in bytes.</summary>
    public long SizeBytes { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this material is visible to all students
    /// enrolled in the course, or only those who attended the specific lesson.
    /// </summary>
    public bool IsPublic { get; private set; }

    /// <summary>
    /// Creates a material attached to a specific theory lesson.
    /// </summary>
    /// <param name="lessonId">The parent lesson identifier.</param>
    /// <param name="title">The material display name.</param>
    /// <param name="fileUrl">The storage URL of the file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="sizeBytes">The file size in bytes.</param>
    /// <param name="isPublic">Whether the material is publicly visible to all students.</param>
    public static LessonMaterial ForLesson(
        Guid lessonId,
        string title,
        string fileUrl,
        string contentType,
        long sizeBytes,
        bool isPublic = false)
        => new()
        {
            LessonId = lessonId,
            Title = title,
            FileUrl = fileUrl,
            ContentType = contentType,
            SizeBytes = sizeBytes,
            IsPublic = isPublic
        };

    /// <summary>
    /// Creates a material attached to a course topic, available to all students
    /// studying that topic regardless of which lesson they attended.
    /// </summary>
    /// <param name="topicId">The parent topic identifier.</param>
    /// <param name="title">The material display name.</param>
    /// <param name="fileUrl">The storage URL of the file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="sizeBytes">The file size in bytes.</param>
    public static LessonMaterial ForTopic(
        Guid topicId,
        string title,
        string fileUrl,
        string contentType,
        long sizeBytes)
        => new()
        {
            TopicId = topicId,
            Title = title,
            FileUrl = fileUrl,
            ContentType = contentType,
            SizeBytes = sizeBytes,
            IsPublic = true
        };
}
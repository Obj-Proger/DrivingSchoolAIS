using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a single topic within a <see cref="Course"/>.
/// Topics define the structured curriculum and are studied sequentially.
/// </summary>
public sealed class CourseTopic : BaseEntity
{
    private CourseTopic() { } // Required by EF Core

    /// <summary>Gets the identifier of the course this topic belongs to.</summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets the display order of this topic within the course.
    /// Managed automatically by <see cref="Course.AddTopic"/>.
    /// </summary>
    public int OrderIndex { get; private set; }

    /// <summary>Gets the title of the topic.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets an optional extended description of the topic.</summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new course topic. The order index is assigned by the parent
    /// <see cref="Course"/> through <see cref="SetOrder"/>.
    /// </summary>
    /// <param name="courseId">The identifier of the parent course.</param>
    /// <param name="title">The topic title.</param>
    /// <param name="description">An optional description.</param>
    public static CourseTopic Create(Guid courseId, string title, string? description = null)
        => new()
        {
            CourseId = courseId,
            Title = title,
            Description = description,
            OrderIndex = 0 // Assigned by Course.AddTopic
        };

    /// <summary>
    /// Sets the display order. Called exclusively by <see cref="Course.AddTopic"/>.
    /// </summary>
    internal void SetOrder(int order) => OrderIndex = order;

    /// <summary>Updates the topic's title and description.</summary>
    public void Update(string title, string? description)
    {
        Title = title;
        Description = description;
    }
}
using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a timestamped note added to a <see cref="Lead"/> by a staff member.
/// Notes are immutable after creation.
/// </summary>
public sealed class LeadNote : BaseEntity
{
    private LeadNote() { } // Required by EF Core

    /// <summary>Gets the identifier of the lead this note belongs to.</summary>
    public Guid LeadId { get; private set; }

    /// <summary>Gets the text content of the note.</summary>
    public string Text { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the staff member who created the note.</summary>
    public Guid AuthorId { get; private set; }

    /// <summary>
    /// Creates a new note for the specified lead.
    /// </summary>
    /// <param name="leadId">The identifier of the parent lead.</param>
    /// <param name="text">The note content.</param>
    /// <param name="authorId">The identifier of the note's author.</param>
    public static LeadNote Create(Guid leadId, string text, Guid authorId)
        => new()
        {
            LeadId = leadId,
            Text = text,
            AuthorId = authorId
        };
}
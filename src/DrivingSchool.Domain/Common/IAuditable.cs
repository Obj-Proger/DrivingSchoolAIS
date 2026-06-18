namespace DrivingSchool.Domain.Common;

/// <summary>
/// Defines audit timestamp properties automatically populated
/// by the EF Core <c>AuditSaveChangesInterceptor</c>.
/// </summary>
public interface IAuditable
{
    /// <summary>Gets or sets the UTC timestamp when the entity was created.</summary>
    DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the UTC timestamp when the entity was last updated.</summary>
    DateTime UpdatedAt { get; set; }
}
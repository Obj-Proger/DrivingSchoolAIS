namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Identifies the kind of content carried by a <see cref="DrivingSchool.Domain.Entities.Message"/>.
/// </summary>
public enum MessageContentType
{
    /// <summary>A plain text message. Content is AES-256-GCM encrypted.</summary>
    Text = 1,

    /// <summary>An image attachment. The file URL is stored separately.</summary>
    Image = 2,

    /// <summary>A generic file attachment. The file URL is stored separately.</summary>
    File = 3,

    /// <summary>
    /// An automated system message that may carry an interactive action button.
    /// </summary>
    System = 4
}
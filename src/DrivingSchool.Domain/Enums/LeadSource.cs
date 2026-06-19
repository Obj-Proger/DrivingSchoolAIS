namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Identifies the channel through which a potential student was acquired.
/// Used to measure the effectiveness of marketing sources.
/// </summary>
public enum LeadSource
{
    /// <summary>The lead submitted a form on the school's website.</summary>
    Website = 1,

    /// <summary>The lead called the school directly.</summary>
    Phone = 2,

    /// <summary>The lead was referred by an existing student or partner.</summary>
    Referral = 3,

    /// <summary>The lead came from the "Хочу Водить" mobile application.</summary>
    WantDriveApp = 4,

    /// <summary>The lead came from an online or offline advertisement.</summary>
    Advertisement = 5,

    /// <summary>The lead came from a social media platform.</summary>
    SocialMedia = 6,

    /// <summary>The source is not covered by the other values.</summary>
    Other = 7
}
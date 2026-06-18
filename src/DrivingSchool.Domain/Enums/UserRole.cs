namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the roles available within the driving school system.
/// Each role determines the set of operations a user is authorised to perform.
/// </summary>
public enum UserRole
{
    /// <summary>Full system access: users, courses, settings, audit log.</summary>
    Admin = 1,

    /// <summary>CRM, contracts, payments, scheduling, reports, staff tasks.</summary>
    Manager = 2,

    /// <summary>Practice slot management, student cards, skill assessment.</summary>
    Instructor = 3,

    /// <summary>Theory lessons, question bank, test templates.</summary>
    Teacher = 4,

    /// <summary>Schedule view, lesson booking, online learning, tests, payments.</summary>
    Student = 5
}
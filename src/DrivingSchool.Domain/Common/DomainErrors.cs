namespace DrivingSchool.Domain.Common;

/// <summary>
/// Centralised catalogue of domain errors grouped by aggregate or value object.
/// Each nested static class corresponds to a specific domain concept.
/// </summary>
/// <remarks>
/// New error groups are added as new aggregates are introduced.
/// Error codes follow the convention: <c>Concept.ErrorName</c>.
/// </remarks>
public static class DomainErrors
{
    /// <summary>Errors related to the <c>Email</c> value object.</summary>
    public static class Email
    {
        public static readonly Error Empty =
            new("Email.Empty", "Email address must not be empty.");

        public static readonly Error TooLong =
            new("Email.TooLong", "Email address must not exceed 320 characters.");

        public static readonly Error InvalidFormat =
            new("Email.InvalidFormat", "Email address has an invalid format.");
    }

    /// <summary>Errors related to the <c>FullName</c> value object.</summary>
    public static class FullName
    {
        public static readonly Error FirstNameEmpty =
            new("FullName.FirstNameEmpty", "First name must not be empty.");

        public static readonly Error LastNameEmpty =
            new("FullName.LastNameEmpty", "Last name must not be empty.");

        public static readonly Error FieldTooLong =
            new("FullName.FieldTooLong",
                "First name, last name and middle name must not exceed 50 characters.");
    }

    /// <summary>Errors related to the <c>PhoneNumber</c> value object.</summary>
    public static class PhoneNumber
    {
        public static readonly Error Empty =
            new("PhoneNumber.Empty", "Phone number must not be empty.");

        public static readonly Error InvalidFormat =
            new("PhoneNumber.InvalidFormat",
                "Phone number has an invalid format. " +
                "Accepted formats: +7XXXXXXXXXX, 8XXXXXXXXXX, XXXXXXXXXX (10 digits).");
    }

    /// <summary>Errors related to the <c>Money</c> value object.</summary>
    public static class Money
    {
        public static readonly Error NegativeAmount =
            new("Money.NegativeAmount", "Monetary amount must not be negative.");

        public static readonly Error InvalidCurrency =
            new("Money.InvalidCurrency", "Currency code must not be empty.");
    }

    /// <summary>Errors related to the <c>User</c> aggregate.</summary>
    public static class User
    {
        public static readonly Error NotFound =
            Error.NotFound("User");

        public static readonly Error AlreadyInactive =
            new("User.AlreadyInactive", "The user account is already inactive.");

        public static readonly Error EmailAlreadyTaken =
            new("User.EmailAlreadyTaken", "An account with this email address already exists.");
    }

    /// <summary>Errors related to authentication and token management.</summary>
    public static class Auth
    {
        public static readonly Error InvalidCredentials =
            new("Auth.InvalidCredentials", "Invalid email address or password.");

        public static readonly Error InvalidRefreshToken =
            new("Auth.InvalidRefreshToken", "The refresh token is invalid.");

        public static readonly Error TokenAlreadyRevoked =
            new("Auth.TokenAlreadyRevoked", "The refresh token has already been revoked.");

        public static readonly Error TokenExpired =
            new("Auth.TokenExpired", "The token has expired.");

        public static readonly Error InvalidToken =
            new("Auth.InvalidToken", "The token is invalid or does not match.");

        public static readonly Error EmailAlreadyConfirmed =
            new("Auth.EmailAlreadyConfirmed", "The email address has already been confirmed.");

        public static readonly Error AccountInactive =
            new("Auth.AccountInactive", "The user account has been deactivated.");
    }

    /// <summary>Errors related to the <c>Lead</c> aggregate.</summary>
    public static class Lead
    {
        public static readonly Error NotFound =
            Error.NotFound("Lead");

        public static readonly Error AlreadyInTerminalState =
            new("Lead.AlreadyInTerminalState",
                "Cannot modify a lead that has already been converted or marked as lost.");
    }

    /// <summary>Errors related to the <c>StaffTask</c> aggregate.</summary>
    public static class StaffTask
    {
        public static readonly Error NotFound =
            Error.NotFound("StaffTask");

        public static readonly Error AlreadyCompleted =
            new("StaffTask.AlreadyCompleted", "The task has already been completed.");
    }

    /// <summary>Errors related to the <c>Course</c> aggregate.</summary>
    public static class Course
    {
        public static readonly Error NotFound =
            Error.NotFound("Course");

        public static readonly Error InvalidHours =
            new("Course.InvalidHours",
                "Theory and practice hours must each be greater than zero.");

        public static readonly Error Inactive =
            new("Course.Inactive",
                "Cannot create a contract for an inactive course.");
    }

    /// <summary>Errors related to the <c>Contract</c> aggregate.</summary>
    public static class Contract
    {
        public static readonly Error NotFound =
            Error.NotFound("Contract");

        public static readonly Error NumberEmpty =
            new("Contract.NumberEmpty", "Contract number must not be empty.");

        public static readonly Error InvalidDateRange =
            new("Contract.InvalidDateRange",
                "The end date must be later than the start date.");

        public static readonly Error AlreadyClosed =
            new("Contract.AlreadyClosed",
                "The contract has already been completed or terminated.");
    }

    /// <summary>Errors related to the <c>Group</c> aggregate.</summary>
    public static class Group
    {
        public static readonly Error NotFound =
            Error.NotFound("Group");

        public static readonly Error Full =
            new("Group.Full",
                "The group has reached its maximum capacity or is no longer accepting members.");

        public static readonly Error AlreadyMember =
            new("Group.AlreadyMember",
                "This contract is already enrolled in the group.");

        public static readonly Error MemberNotFound =
            new("Group.MemberNotFound",
                "The specified contract is not a member of this group.");

        public static readonly Error InvalidCapacity =
            new("Group.InvalidCapacity",
                "Maximum student count must be greater than zero.");
    }

    /// <summary>Errors related to the <c>TheoryLesson</c> aggregate.</summary>
    public static class TheoryLesson
    {
        public static readonly Error NotFound =
            Error.NotFound("TheoryLesson");

        public static readonly Error InvalidDuration =
            new("TheoryLesson.InvalidDuration",
                "Lesson duration must be greater than zero minutes.");

        public static readonly Error PastScheduledTime =
            new("TheoryLesson.PastScheduledTime",
                "A lesson cannot be scheduled in the past.");

        public static readonly Error CannotComplete =
            new("TheoryLesson.CannotComplete",
                "Only a scheduled lesson can be marked as completed.");

        public static readonly Error CannotCancel =
            new("TheoryLesson.CannotCancel",
                "Only a scheduled lesson can be cancelled.");

        public static readonly Error CannotReschedule =
            new("TheoryLesson.CannotReschedule",
                "Only a scheduled lesson can be rescheduled or updated.");

        public static readonly Error IsCancelled =
            new("TheoryLesson.IsCancelled",
                "Cannot modify a cancelled lesson.");

        public static readonly Error MaterialNotFound =
            new("TheoryLesson.MaterialNotFound",
                "The specified material was not found in this lesson.");
    }

    /// <summary>Errors related to the <c>PracticeSlot</c> aggregate.</summary>
    public static class PracticeSlot
    {
        public static readonly Error NotFound =
            Error.NotFound("PracticeSlot");

        public static readonly Error InvalidTimeRange =
            new("PracticeSlot.InvalidTimeRange",
                "The end time must be later than the start time.");

        public static readonly Error NotAvailable =
            new("PracticeSlot.NotAvailable",
                "The slot is not available for booking.");

        public static readonly Error NotBooked =
            new("PracticeSlot.NotBooked",
                "The slot is not currently booked and cannot be released.");

        public static readonly Error AlreadyCompleted =
            new("PracticeSlot.AlreadyCompleted",
                "A completed slot cannot be cancelled.");
    }

    /// <summary>Errors related to the <c>PracticeBooking</c> aggregate.</summary>
    public static class PracticeBooking
    {
        public static readonly Error NotFound =
            Error.NotFound("PracticeBooking");

        public static readonly Error CannotCancel =
            new("PracticeBooking.CannotCancel",
                "Only a confirmed booking can be cancelled.");

        public static readonly Error CannotComplete =
            new("PracticeBooking.CannotComplete",
                "Only a confirmed booking can be marked as completed.");

        public static readonly Error CannotRate =
            new("PracticeBooking.CannotRate",
                "Only a completed booking can be rated.");

        public static readonly Error AlreadyRated =
            new("PracticeBooking.AlreadyRated",
                "This booking has already been rated.");

        public static readonly Error InvalidRating =
            new("PracticeBooking.InvalidRating",
                "Rating must be a value between 1 and 5.");

        public static readonly Error InvalidSkillName =
            new("PracticeBooking.InvalidSkillName",
                "Skill name must not be empty.");
    }
}
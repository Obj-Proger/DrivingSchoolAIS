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
}
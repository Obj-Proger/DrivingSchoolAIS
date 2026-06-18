namespace DrivingSchool.Domain.Common;

/// <summary>
/// Represents a domain error with a machine-readable code and a human-readable message.
/// Errors are used as the failure state inside <see cref="Result"/> and <see cref="Result{TValue}"/>.
/// </summary>
/// <param name="Code">
/// Machine-readable error identifier following the convention <c>Entity.ErrorName</c>,
/// e.g. <c>User.NotFound</c> or <c>Email.InvalidFormat</c>.
/// </param>
/// <param name="Message">Human-readable error description.</param>
public sealed record Error(string Code, string Message)
{
    /// <summary>Represents the absence of an error. Used for successful results.</summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>Creates a standardised not-found error for the given entity.</summary>
    /// <param name="entityName">The name of the entity that was not found.</param>
    public static Error NotFound(string entityName) =>
        new($"{entityName}.NotFound", $"{entityName} was not found.");

    /// <summary>Creates a standardised conflict error for the given entity.</summary>
    /// <param name="entityName">The name of the conflicting entity.</param>
    public static Error Conflict(string entityName) =>
        new($"{entityName}.Conflict", $"{entityName} already exists.");
}
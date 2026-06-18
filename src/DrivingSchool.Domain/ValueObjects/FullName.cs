using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.ValueObjects;

/// <summary>
/// Represents a person's full name consisting of a first name,
/// last name, and an optional middle name (patronymic).
/// </summary>
public sealed class FullName : ValueObject
{
    private FullName(string firstName, string lastName, string? middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    /// <summary>Gets the person's first name.</summary>
    public string FirstName { get; }

    /// <summary>Gets the person's last name (family name).</summary>
    public string LastName { get; }

    /// <summary>Gets the person's middle name (patronymic), or <c>null</c> if not provided.</summary>
    public string? MiddleName { get; }

    /// <summary>
    /// Gets the full display name in the format <c>LastName FirstName MiddleName</c>,
    /// or <c>LastName FirstName</c> when no middle name is present.
    /// </summary>
    public string DisplayName => MiddleName is null
        ? $"{LastName} {FirstName}"
        : $"{LastName} {FirstName} {MiddleName}";

    /// <summary>
    /// Gets an abbreviated name in the format <c>LastName F. M.</c>,
    /// or <c>LastName F.</c> when no middle name is present.
    /// </summary>
    public string ShortName => MiddleName is null
        ? $"{LastName} {FirstName[0]}."
        : $"{LastName} {FirstName[0]}. {MiddleName[0]}.";

    /// <summary>
    /// Creates a validated <see cref="FullName"/> instance.
    /// All provided components are trimmed.
    /// </summary>
    /// <param name="firstName">The person's first name. Required.</param>
    /// <param name="lastName">The person's last name. Required.</param>
    /// <param name="middleName">The person's middle name (patronymic). Optional.</param>
    /// <returns>
    /// A successful <see cref="Result{FullName}"/> or a failure describing
    /// which validation rule was violated.
    /// </returns>
    public static Result<FullName> Create(
        string? firstName,
        string? lastName,
        string? middleName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FullName>(DomainErrors.FullName.FirstNameEmpty);

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<FullName>(DomainErrors.FullName.LastNameEmpty);

        var trimmedFirst = firstName.Trim();
        var trimmedLast = lastName.Trim();
        var trimmedMiddle = string.IsNullOrWhiteSpace(middleName)
            ? null
            : middleName.Trim();

        if (trimmedFirst.Length > 50 || trimmedLast.Length > 50)
            return Result.Failure<FullName>(DomainErrors.FullName.FieldTooLong);

        if (trimmedMiddle is not null && trimmedMiddle.Length > 50)
            return Result.Failure<FullName>(DomainErrors.FullName.FieldTooLong);

        return Result.Success(new FullName(trimmedFirst, trimmedLast, trimmedMiddle));
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }

    /// <inheritdoc />
    public override string ToString() => DisplayName;
}
using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a training programme offered by the driving school.
/// A course defines the curriculum, duration, and price for a specific licence category.
/// </summary>
public sealed class Course : BaseEntity
{
    private readonly List<CourseTopic> _topics = [];

    private Course() { } // Required by EF Core

    /// <summary>Gets the name of the course (e.g. "Category B — Full programme").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets an optional description of the course content.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets the driving licence category this course prepares students for.</summary>
    public LicenseCategory Category { get; private set; }

    /// <summary>Gets the total number of theory lesson hours in this course.</summary>
    public int TheoryHoursTotal { get; private set; }

    /// <summary>Gets the total number of practical driving hours in this course.</summary>
    public int PracticeHoursTotal { get; private set; }

    /// <summary>Gets the current price of this course.</summary>
    public Money Price { get; private set; } = null!;

    /// <summary>Gets a value indicating whether this course is available for new contracts.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the ordered list of topics that make up this course's curriculum.</summary>
    public IReadOnlyList<CourseTopic> Topics => _topics.AsReadOnly();

    // Factory

    /// <summary>
    /// Creates a new active course.
    /// </summary>
    /// <param name="name">The course name.</param>
    /// <param name="category">The target licence category.</param>
    /// <param name="theoryHoursTotal">Total theory hours.</param>
    /// <param name="practiceHoursTotal">Total practice hours.</param>
    /// <param name="price">The course price.</param>
    /// <param name="description">An optional description.</param>
    /// <returns>
    /// A successful <see cref="Result{Course}"/>,
    /// or a failure with <see cref="DomainErrors.Course.InvalidHours"/>.
    /// </returns>
    public static Result<Course> Create(
        string name,
        LicenseCategory category,
        int theoryHoursTotal,
        int practiceHoursTotal,
        Money price,
        string? description = null)
    {
        if (theoryHoursTotal <= 0 || practiceHoursTotal <= 0)
            return Result.Failure<Course>(DomainErrors.Course.InvalidHours);

        return Result.Success(new Course
        {
            Name = name,
            Category = category,
            TheoryHoursTotal = theoryHoursTotal,
            PracticeHoursTotal = practiceHoursTotal,
            Price = price,
            Description = description,
            IsActive = true
        });
    }

    // Behaviour

    /// <summary>
    /// Appends a topic to the end of the course curriculum.
    /// The topic's order index is set automatically.
    /// </summary>
    /// <param name="topic">The topic to add.</param>
    public void AddTopic(CourseTopic topic)
    {
        topic.SetOrder(_topics.Count + 1);
        _topics.Add(topic);
    }

    /// <summary>Updates the course price.</summary>
    /// <param name="price">The new price.</param>
    public void UpdatePrice(Money price) => Price = price;

    /// <summary>Updates the course name and description.</summary>
    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Deactivates the course, preventing it from being selected for new contracts.
    /// Existing active contracts are not affected.
    /// </summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Reactivates a previously deactivated course.</summary>
    public void Activate() => IsActive = true;
}
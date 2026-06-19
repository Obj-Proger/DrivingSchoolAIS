using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a predefined driving route used during practical lessons.
/// Routes are created by instructors and attached to completed bookings
/// so students can review the path they drove.
/// </summary>
public sealed class DrivingRoute : BaseEntity
{
    private DrivingRoute() { } // Required by EF Core

    /// <summary>Gets the identifier of the instructor who created this route.</summary>
    public Guid InstructorId { get; private set; }

    /// <summary>Gets the display name of the route (e.g. "City Centre Circuit").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets a text description of the route's key waypoints and manoeuvres.</summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Gets optional GeoJSON or encoded polyline data for map rendering,
    /// or <c>null</c> if only a text description is available.
    /// </summary>
    public string? MapData { get; private set; }

    /// <summary>Creates a new driving route.</summary>
    /// <param name="instructorId">The creating instructor's identifier.</param>
    /// <param name="name">The route name.</param>
    /// <param name="description">A text description of the route.</param>
    /// <param name="mapData">Optional GeoJSON or polyline map data.</param>
    public static DrivingRoute Create(
        Guid instructorId,
        string name,
        string description,
        string? mapData = null)
        => new()
        {
            InstructorId = instructorId,
            Name = name.Trim(),
            Description = description.Trim(),
            MapData = mapData
        };

    /// <summary>Updates the route's details.</summary>
    public void Update(string name, string description, string? mapData)
    {
        Name = name.Trim();
        Description = description.Trim();
        MapData = mapData;
    }
}
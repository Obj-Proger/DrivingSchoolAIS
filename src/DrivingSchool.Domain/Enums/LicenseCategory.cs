namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the vehicle licence categories as specified
/// by the Russian Federation traffic law (Federal Law No. 196-FZ).
/// </summary>
public enum LicenseCategory
{
    /// <summary>Motorcycles with engine capacity over 125 cc.</summary>
    A = 1,

    /// <summary>Light motorcycles with engine capacity up to 125 cc.</summary>
    A1 = 2,

    /// <summary>Passenger cars and light goods vehicles up to 3.5 t.</summary>
    B = 3,

    /// <summary>Three-wheeled motor vehicles and quadricycles.</summary>
    B1 = 4,

    /// <summary>Category B vehicles with a trailer exceeding 750 kg.</summary>
    BE = 5,

    /// <summary>Goods vehicles with a maximum authorised mass over 3.5 t.</summary>
    C = 6,

    /// <summary>Goods vehicles with a maximum authorised mass between 3.5 t and 7.5 t.</summary>
    C1 = 7,

    /// <summary>Category C vehicles with a trailer.</summary>
    CE = 8,

    /// <summary>Category C1 vehicles with a trailer.</summary>
    C1E = 9,

    /// <summary>Motor vehicles designed for carrying more than eight passengers.</summary>
    D = 10,

    /// <summary>Motor vehicles designed for carrying 8 to 16 passengers.</summary>
    D1 = 11,

    /// <summary>Category D vehicles with a trailer.</summary>
    DE = 12,

    /// <summary>Category D1 vehicles with a trailer.</summary>
    D1E = 13,

    /// <summary>Mopeds and light quadricycles.</summary>
    M = 14,

    /// <summary>Trams.</summary>
    Tm = 15,

    /// <summary>Trolleybuses.</summary>
    Tb = 16
}
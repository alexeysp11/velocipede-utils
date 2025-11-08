namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Working hours.
/// </summary>
public class WorkingHours
{
    /// <summary>
    /// Number of hours.
    /// </summary>
    public int Hours { get; set; }

    /// <summary>
    /// Number of hours on business trip.
    /// </summary>
    public int HourOnBusinessTrip { get; set; }

    /// <summary>
    /// Number of hours on holidays.
    /// </summary>
    public int HourOnHolidays { get; set; }

    /// <summary>
    /// Extra hours.
    /// </summary>
    public int ExtraHours { get; set; }
}
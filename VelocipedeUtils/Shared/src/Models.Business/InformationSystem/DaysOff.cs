namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Days off.
/// </summary>
public class DaysOff
{
    /// <summary>
    /// Number of paid days off.
    /// </summary>
    public int PaidDaysOff { get; set; }

    /// <summary>
    /// Number of unpaid days off.
    /// </summary>
    public int UnpaidDaysOff { get; set; }

    /// <summary>
    /// Number of sick days off.
    /// </summary>
    public int SickDaysOff { get; set; }
}
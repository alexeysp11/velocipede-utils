namespace VelocipedeUtils.Shared.Models.Business.Monetary;

/// <summary>
/// Sub total payments.
/// </summary>
public class SubTotalPayments
{
    public decimal PayForHours { get; private set; }

    public decimal PayForBusinessTrip { get; private set; }

    public decimal PayForExtraHours { get; private set; }

    public decimal PayForHolidayHours { get; private set; }

    public decimal PayForPayedDaysOff { get; private set; }
}
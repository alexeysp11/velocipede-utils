namespace VelocipedeUtils.Shared.Models.Business.Monetary;

/// <summary>
/// Pay rate.
/// </summary>
public class PayRate : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Boolean variable that shows if this is a base rate.
    /// </summary>
    public bool IsBaseRate { get; private set; }

    /// <summary>
    /// Hour rate.
    /// </summary>
    public decimal? HourRate { get; private set; }

    /// <summary>
    /// Extra hour rate.
    /// </summary>
    public decimal? ExtraHourRate { get; private set; }

    /// <summary>
    /// Hoiday hour rate.
    /// </summary>
    public decimal? HoidayHourRate { get; private set; }

    /// <summary>
    /// Business trip hour rate.
    /// </summary>
    public decimal? BusinessTripHourRate { get; private set; }
}
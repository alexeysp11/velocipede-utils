namespace VelocipedeUtils.Shared.Models.Business;

/// <summary>
/// Interface that represents a business entity which is temporal.
/// </summary>
public interface ITemporalBusinessEntity : IWfBusinessEntity
{
    /// <summary>
    /// Actual start date.
    /// </summary>
    DateTime? DateStartActual { get; set; }
    
    /// <summary>
    /// Actual end date.
    /// </summary>
    DateTime? DateEndActual { get; set; }
    
    /// <summary>
    /// Expected start date.
    /// </summary>
    DateTime? DateStartExpected { get; set; }
    
    /// <summary>
    /// Expected end date.
    /// </summary>
    DateTime? DateEndExpected { get; set; }
}
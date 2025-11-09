namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Absence of an employee.
/// </summary>
public class Absense : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// Employee that is absent.
    /// </summary>
    public Employee? Employee { get; set; }
    
    /// <summary>
    /// Reason of the absence.
    /// </summary>
    public AbsenseReason Reason { get; set; }
    
    /// <summary>
    /// Status of the absence.
    /// </summary>
    public AbsenseStatus Status { get; set; }
    
    /// <summary>
    /// Displays whether employee absence needs to be paid.
    /// </summary>
    public bool? IsPaid { get; set; }
    
    /// <summary>
    /// Actual start date.
    /// </summary>
    public DateTime? DateStartActual { get; set; }
    
    /// <summary>
    /// Actual end date.
    /// </summary>
    public DateTime? DateEndActual { get; set; }
    
    /// <summary>
    /// Expected start date.
    /// </summary>
    public DateTime? DateStartExpected { get; set; }
    
    /// <summary>
    /// Expected end date.
    /// </summary>
    public DateTime? DateEndExpected { get; set; }
}
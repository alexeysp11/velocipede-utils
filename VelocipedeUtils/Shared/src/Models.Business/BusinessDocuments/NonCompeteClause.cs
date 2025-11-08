namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Non-complete clause.
/// </summary>
public class NonCompeteClause : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// Geographic area.
    /// </summary>
    public string? GeographicArea { get; set; }

    /// <summary>
    /// ???
    /// </summary>
    public string? LimitedScope { get; set; }

    /// <summary>
    /// ???
    /// </summary>
    public string? LegitimateCompetitiveInterest { get; set; }

    /// <summary>
    /// ??? 
    /// </summary>
    public string? EmployeeCompensation { get; set; }
    
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
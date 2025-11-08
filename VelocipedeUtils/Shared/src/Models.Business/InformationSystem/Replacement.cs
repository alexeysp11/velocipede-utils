namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Replacement.
/// </summary>
public class Replacement : WfBusinessEntity, IWfBusinessEntity, ITemporalBusinessEntity
{
    /// <summary>
    /// The number of working days for which the replacement mechanism must be completed 
    /// (reassigning tasks, informing users, etc.) before the actual start date of replacement.
    /// </summary>
    public int PreReplace { get; set; }
    
    /// <summary>
    /// Whether or not to duplicate messages to the Substitute, if "Yes" - the messages go to the Substitute and the Substitute.
    /// </summary>
    public bool DuplicateMessage { get; set; }
    
    /// <summary>
    /// User who is replaced.
    /// </summary>
    public UserAccount? SourceUser { get; set; }
    
    /// <summary>
    /// User who is replacing the absent user.
    /// </summary>
    public UserAccount? TargetUser { get; set; }
    
    /// <summary>
    /// Status of the replacement.
    /// </summary>
    public ReplacementStatus Status { get; set; }
    
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
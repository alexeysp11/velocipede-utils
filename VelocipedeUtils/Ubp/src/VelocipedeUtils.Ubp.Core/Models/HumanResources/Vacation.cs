namespace VelocipedeUtils.Ubp.Core.Models.HumanResources;

/// <summary>
/// Represents a vacation.
/// </summary>
public class Vacation
{
    /// <summary>
    /// Date when the vacation starts.
    /// </summary>
    public System.DateTime BeginDate { get; set; }
    
    /// <summary>
    /// Date when the vacation ends.
    /// </summary>
    public System.DateTime EndDate { get; set; }
    
    /// <summary>
    /// The employee that the vacation is mapped to.
    /// </summary>
    public Employee Employee { get; set; }
}
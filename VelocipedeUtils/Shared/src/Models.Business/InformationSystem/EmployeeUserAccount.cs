namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Intermediate table between <see cref="Employee"/> and <see cref="UserAccount"/>.
/// </summary>
public class EmployeeUserAccount : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Employee.
    /// </summary>
    public Employee? Employee { get; set; }

    /// <summary>
    /// User account.
    /// </summary>
    public UserAccount? UserAccount { get; set; }
}
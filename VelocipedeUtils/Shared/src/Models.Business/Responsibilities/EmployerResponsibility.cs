namespace VelocipedeUtils.Shared.Models.Business.Responsibilities;

/// <summary>
/// Employer responsibility.
/// </summary>
public class EmployerResponsibility : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Employer responsibility type.
    /// </summary>
    public EmployerResponsibilityType ResponsibilityType { get; set; }
}
namespace VelocipedeUtils.Ubp.Core.Models.HumanResources;

/// <summary>
/// Represents an employee.
/// </summary>
public class Employee
{
    /// <summary>
    /// Full name of the employee.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Gender of the employee.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Employee's job title.
    /// </summary>
    public JobTitle JobTitle { get; set; }

    /// <summary>
    /// Employee's department.
    /// </summary>
    public Department Department { get; set; }
    
    /// <summary>
    /// Employee's date of birth.
    /// </summary>
    public System.DateTime BirthDate { get; set; }
}

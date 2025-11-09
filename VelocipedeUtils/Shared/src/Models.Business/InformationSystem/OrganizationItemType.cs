using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Type of the organization item.
/// </summary>
public enum OrganizationItemType
{
    [Display(Name = "Department")]
    Department = 0,
    
    [Display(Name = "Job position")]
    JobPosition = 1,
    
    [Display(Name = "Employee group")]
    EmployeeGroup = 2,
    
    [Display(Name = "Nested structure")]
    NestedStructure = 3,
    
    [Display(Name = "Team")]
    Team = 4
}
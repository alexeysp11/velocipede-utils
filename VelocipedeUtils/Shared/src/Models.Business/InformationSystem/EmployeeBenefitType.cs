using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Employee benifit type.
/// </summary>
public enum EmployeeBenefitType
{
    [Display(Name = "Not Selected")]
    None = 0,

    [Display(Name = "Child Care")]
    ChildCare = 1,

    [Display(Name = "Health Insurance")]
    HealthInsurance = 2,

    [Display(Name = "Dental Insurance")]
    DentalInsurance = 3,

    [Display(Name = "Life Insurance")]
    LifeInsurance = 4,

    [Display(Name = "Retirement Plan")]
    RetirementPlan = 5,

    [Display(Name = "Vision Care")]
    VisionCare = 6,

    [Display(Name = "Fitness")]
    Fitness = 7,

    [Display(Name = "Paid Vacation Leave")]
    PaidVacationLeave = 8,

    [Display(Name = "Personal Leave")]
    PersonalLeave = 9,

    [Display(Name = "Sick Leave")]
    SickLeave = 10
}
using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Employment termination.
/// </summary>
public enum EmploymentTerminationReason
{
    [Display(Name = "Not Selected")]
    None = 0,

    [Display(Name = "Poor Quality Of Work")]
    PoorQualityOfWork = 1,

    [Display(Name = "Interpersonal Conflicts")]
    InterpersonalConflicts = 2,

    [Display(Name = "Medical Reasons")]
    MedicalReasons = 3,

    [Display(Name = "Difficult Work Schedule/Environment")]
    DifficultWorkScheduleEnvironment = 4,

    [Display(Name = "Difficult Work Schedule/Environment")]
    JobDissatisfaction = 5,

    [Display(Name = "Mental Illness")]
    MentalIllness = 6,

    [Display(Name = "Performance")]
    Performance = 7,

    [Display(Name = "Dependability")]
    Dependability = 8,

    [Display(Name = "Transportation Issue")]
    TransportationIssue = 9,

    [Display(Name = "Begin A New Job")]
    BeginNewJob = 10,

    [Display(Name = "Family Issues")]
    FamilyIssues = 11,

    [Display(Name = "Low Salary")]
    LowSalary = 12,

    [Display(Name = "Fear")]
    Fear = 13,

    [Display(Name = "Lack Supervision")]
    LackSupervision = 14,

    [Display(Name = "Substance Abuse")]
    SubstanceAbuse = 15,

    [Display(Name = "Mutual Agreement")]
    MutualAgreement = 16,

    [Display(Name = "Layoff")]
    Layoff = 17,

    [Display(Name = "Furlough")]
    Furlough = 18,

    [Display(Name = "Job Elimination")]
    JobElimination = 19,

    [Display(Name = "Reduction In Force")]
    ReductionInForce = 20,

    [Display(Name = "Retirement")]
    Retirement = 21,

    [Display(Name = "Lockout")]
    Lockout = 22,

    [Display(Name = "Expired Employment Contract")]
    ExpiredEmploymentContract = 23,

    [Display(Name = "End Of Seasonal Employment")]
    EndOfSeasonalEmployment = 24,

    [Display(Name = "Military Call-Up")]
    MilitaryCallUp = 25,

    [Display(Name = "Job Abondonment")]
    JobAbondonment = 26,

    [Display(Name = "Employee Initiated Strike")]
    EmployeeInitiatedStrike = 27,

    [Display(Name = "Other Reasons")]
    OtherReasons = 28
}
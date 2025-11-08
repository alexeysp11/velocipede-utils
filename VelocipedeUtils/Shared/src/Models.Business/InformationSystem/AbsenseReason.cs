using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.InformationSystem;

/// <summary>
/// Reason of the absence.
/// </summary>
public enum AbsenseReason
{
    [Display(Name = "Not Selected")]
    None = -1,

    [Display(Name = "Skipping")]
    Skipping = 0,

    [Display(Name = "Unpaid time off")]
    UnpaidTimeOff = 1,

    [Display(Name = "Paid time off")]
    PaidTimeOff = 2,

    [Display(Name = "Annual leave")]
    AnnualLeave = 3,

    [Display(Name = "Family and medical leave")]
    FamilyMedicalLeave = 4,

    [Display(Name = "Disability leave")]
    DisabilityLeave = 5,

    [Display(Name = "Sabbatical")]
    Sabbatical = 6,

    [Display(Name = "Community service")]
    CommunityService = 7,

    [Display(Name = "Military service")]
    MilitaryService = 8,

    [Display(Name = "Travel")]
    Travel = 9,

    [Display(Name = "Business trip")]
    BusinessTrip = 10,

    [Display(Name = "Suspension")]
    Suspension = 11,

    [Display(Name = "Personal leave")]
    PersonalLeave = 12,

    [Display(Name = "Bereavement leave")]
    BereavementLeave = 13,

    [Display(Name = "Jury duty")]
    JuryDuty = 14,

    [Display(Name = "Health issues")]
    HealthIssues = 15,

    [Display(Name = "Childcare leave")]
    ChildcareLeave = 16,

    [Display(Name = "Family responsibility")]
    FamilyResponsibility = 17,

    [Display(Name = "Vacation")]
    Vacation = 18,

    [Display(Name = "Emergency leave")]
    EmergencyLeave = 19,

    [Display(Name = "Study leave")]
    StudyLeave = 20
}
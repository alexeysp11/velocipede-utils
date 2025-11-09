using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Type of the employment contract.
/// </summary>
public enum EmploymentContractType
{
    [Display(Name = "Not Selected")]
    None = 0,

    [Display(Name = "Full Time Contract")]
    FullTimeContract = 1,

    [Display(Name = "Part Time Contract")]
    PartTimeContract = 2,

    [Display(Name = "Fixed Term Contract")]
    FixedTermContract = 3,

    [Display(Name = "No Contract")]
    NoContract = 4,

    [Display(Name = "Voluntary Work")]
    VoluntaryWork = 5,

    [Display(Name = "Contract Of Mandate")]
    ContractOfMandate = 6,

    [Display(Name = "Contract Of Specific Work")]
    ContractOfSpecificWork = 7,

    [Display(Name = "Internship")]
    Internship = 8,

    [Display(Name = "Freelance")]
    Freelance = 9,

    [Display(Name = "Contractor")]
    Contractor = 10,

    [Display(Name = "Agency Workers")]
    AgencyWorkers = 11
}
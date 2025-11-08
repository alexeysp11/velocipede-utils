using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Status of the employment contract.
/// </summary>
public enum EmploymentContractStatus
{
    [Display(Name = "Not Selected")]
    None = -1,

    [Display(Name = "Active")]
    Active = 0,

    [Display(Name = "Delete")]
    Delete = 1,

    [Display(Name = "Pending")]
    Pending = 2
}
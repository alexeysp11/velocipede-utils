using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Order status.
/// </summary>
public enum OrderStatus
{
    [Display(Name = "Deleted")]
    Deleted = -1,

    [Display(Name = "None")]
    None = 0,

    [Display(Name = "Requested")]
    Requested = 1,

    [Display(Name = "In process")]
    InProcess = 2,

    [Display(Name = "Ready to deliver")]
    ReadyToDeliver = 3,

    [Display(Name = "Delivered")]
    Delivered = 4,

    [Display(Name = "Finished")]
    Finished = 5
}
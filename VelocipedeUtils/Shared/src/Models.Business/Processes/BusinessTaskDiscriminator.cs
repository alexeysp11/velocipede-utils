using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.Processes;

/// <summary>
/// Business task discriminator.
/// </summary>
public enum BusinessTaskDiscriminator
{
    /// <summary>
    /// Request from store to the warehouse.
    /// </summary>
    [Display(Name = "RequestStore2Wh")]
    RequestStore2Wh,

    /// <summary>
    /// Confirm delivering from store to the warehouse.
    /// </summary>
    [Display(Name = "ConfirmStore2Wh")]
    ConfirmStore2Wh,

    /// <summary>
    /// Common delivery operation.
    /// </summary>
    [Display(Name = "DeliveryOperation")]
    DeliveryOperation,

    /// <summary>
    /// Common cooking task.
    /// </summary>
    [Display(Name = "CookingOperation")]
    CookingOperation,

    /// <summary>
    /// Common business task.
    /// </summary>
    [Display(Name = "BusinessTask")]
    BusinessTask,

    /// <summary>
    /// Other business task.
    /// </summary>
    [Display(Name = "Other")]
    Other
}
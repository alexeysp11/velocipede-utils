using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.Delivery;

/// <summary>
/// Delivery method type.
/// </summary>
public enum DeliveryMethodType
{
    [Display(Name = "Not Selected")]
    None = 0,

    [Display(Name = "On foot")]
    OnFoot = 1,

    [Display(Name = "Electric scooter")]
    ElectricScooter = 2,
    
    [Display(Name = "Car")]
    Car = 3
}
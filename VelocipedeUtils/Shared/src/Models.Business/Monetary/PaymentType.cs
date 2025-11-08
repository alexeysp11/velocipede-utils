using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.Monetary;

/// <summary>
/// Payment type.
/// </summary>
public enum PaymentType
{
    [Display(Name = "Not Selected")]
    None = -1,

    [Display(Name = "Card")]
    Card = 0,

    [Display(Name = "QR code")]
    QrCode = 1
}
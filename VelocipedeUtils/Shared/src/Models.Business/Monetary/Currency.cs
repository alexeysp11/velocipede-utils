using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.Monetary;

/// <summary>
/// Currency
/// </summary>
public enum Currency 
{
    [Display(Name = "United States Dollar")]
    USD,

    [Display(Name = "Euro")]
    EUR,

    [Display(Name = "Swiss franc")]
    CHF,

    [Display(Name = "Pound sterling")]
    GBP,

    [Display(Name = "Canadian Dollar")]
    CAD,

    [Display(Name = "Australian Dollar")]
    AUD,

    [Display(Name = "New Zealand Dollar")]
    NZD,

    [Display(Name = "Russian Ruble")]
    RUB,

    [Display(Name = "Belarusian ruble")]
    BYN,

    [Display(Name = "Ukrainian hryvnia")]
    UAH,

    [Display(Name = "Polish złoty")]
    PLN,

    [Display(Name = "Turkish lira")]
    TRY,

    [Display(Name = "Georgian Lari")]
    GEL,

    [Display(Name = "Armenian Dram")]
    AMD,

    [Display(Name = "Kazakhstani Tenge")]
    KZT,

    [Display(Name = "Chinese Yuan")]
    CNY,

    [Display(Name = "South Korean won")]
    KRW,

    [Display(Name = "Japanese yen")]
    JPY,

    [Display(Name = "Vietnamese dong")]
    VND,

    [Display(Name = "Singapore dollar")]
    SGD,

    [Display(Name = "Brazilian Real")]
    BRL,

    [Display(Name = "Argentine Peso")]
    ARS,

    [Display(Name = "Peso Uruguayo")]
    UYU,

    [Display(Name = "Mexican Peso")]
    MXN,

    [Display(Name = "Chilean Peso")]
    CLP,

    [Display(Name = "Venezuelan bolívar")]
    VES
}

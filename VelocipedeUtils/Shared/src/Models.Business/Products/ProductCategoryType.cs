using System.ComponentModel.DataAnnotations;

namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Product category type.
/// </summary>
public enum ProductCategoryType
{
    [Display(Name = "None")]
    None, 

    [Display(Name = "Consumer product")]
    ConsumerProduct,

    [Display(Name = "Convenience product")]
    ConvenienceProduct,

    [Display(Name = "Shopping product")]
    ShoppingProduct,

    [Display(Name = "Speciality product")]
    SpecialityProduct,

    [Display(Name = "Unsought product")]
    UnsoughtProduct,

    [Display(Name = "Industrial product")]
    IndustrialProduct,

    [Display(Name = "Capital goods")]
    CapitalGoods,

    [Display(Name = "Raw material")]
    RawMaterial,

    [Display(Name = "Essential equipment")]
    EssentialEquipment,

    [Display(Name = "Component parts")]
    ComponentParts,

    [Display(Name = "Accessory product")]
    AccessoryProduct,

    [Display(Name = "Services")]
    Services,

    [Display(Name = "Operating supplies")]
    OperatingSupplies
}
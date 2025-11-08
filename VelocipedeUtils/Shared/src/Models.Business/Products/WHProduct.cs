using VelocipedeUtils.Shared.Models.Business.MeasurementUnits;

namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Description of the product stored in the warehouse.
/// Includes characteristics (e.g. weight, dimensions, expiration date, manufacturer).
/// Necessary for product identification, management of their characteristics and traceability.
/// </summary>
public class WHProduct : WfBusinessEntity, IWfBusinessEntity, IExpirableProduct
{
    /// <summary>
    /// Code of the warehouse product.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Serial number of the warehouse product.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Product.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Minimal allowed quantity of the product.
    /// </summary>
    public int MinQuantity { get; set; }

    /// <summary>
    /// Maximal allowed quantity of the product.
    /// </summary>
    public int MaxQuantity { get; set; }

    /// <summary>
    /// Weight of the product.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Minimal allowed weight of the product.
    /// </summary>
    public decimal? MinWeight { get; set; }

    /// <summary>
    /// Maximal allowed weight of the product.
    /// </summary>
    public decimal? MaxWeight { get; set; }

    /// <summary>
    /// Weight unit.
    /// </summary>
    public WeightUnit? WeightUnit { get; set; }
    
    /// <summary>
    /// Production date.
    /// </summary>
    public DateTime? DateProduction { get; set; }
    
    /// <summary>
    /// Packaging date.
    /// </summary>
    public DateTime? DatePackaging { get; set; }
    
    /// <summary>
    /// Expiration date.
    /// </summary>
    public DateTime? DateExpiration { get; set; }
}
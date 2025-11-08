using VelocipedeUtils.Shared.Models.Business.Products;

namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// Warehouse item.
/// </summary>
public class WarehouseItem : WfBusinessEntity, IWfBusinessEntity, IExpirableProduct
{
    /// <summary>
    /// Code of the warehouse item.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Warehouse.
    /// </summary>
    public Warehouse? Warehouse { get; set; }

    /// <summary>
    /// Warehouse product.
    /// </summary>
    public WHProduct? WHProduct { get; set; }

    /// <summary>
    /// Product.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Warehouse item status.
    /// </summary>
    public WarehouseItemStatus WarehouseItemStatus { get; set; }
    
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
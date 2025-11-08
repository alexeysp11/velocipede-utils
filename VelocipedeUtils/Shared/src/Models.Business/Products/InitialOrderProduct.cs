using VelocipedeUtils.Shared.Models.Business.BusinessDocuments;
using VelocipedeUtils.Shared.Models.Business.Packing;

namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Initial order product.
/// </summary>
public class InitialOrderProduct : WfBusinessEntity, IWfBusinessEntity, IExpirableProduct
{
    /// <summary>
    /// Code of the initial order product.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Product.
    /// </summary>
    public Product? Product { get; set; }
    
    /// <summary>
    /// InitialOrder.
    /// </summary>
    public InitialOrder? InitialOrder { get; set; }
    
    /// <summary>
    /// Container.
    /// </summary>
    public Container? Container { get; set; }
    
    /// <summary>
    /// Tray.
    /// </summary>
    public Tray? Tray { get; set; }
    
    /// <summary>
    /// Lot.
    /// </summary>
    public Lot? Lot { get; set; }
    
    /// <summary>
    /// Quantity.
    /// </summary>
    public int Quantity { get; set; }
    
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
namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Product attribute.
/// </summary>
public class ProductAttribute : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Product.
    /// </summary>
    public Product? Product { get; set; }
    
    /// <summary>
    /// Attribute name.
    /// </summary>
    public string? AttributeName { get; set; }
    
    /// <summary>
    /// Product attribute type.
    /// </summary>
    public ProductAttributeType? ProductAttributeType { get; set; }
}
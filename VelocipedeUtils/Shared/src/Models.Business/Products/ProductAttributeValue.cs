namespace VelocipedeUtils.Shared.Models.Business.Products;

/// <summary>
/// Product attribute value.
/// </summary>
public class ProductAttributeValue
{
    /// <summary>
    /// Product.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Product attribute.
    /// </summary>
    public ProductAttribute? ProductAttribute { get; set; }

    /// <summary>
    /// Value of the attribute.
    /// </summary>
    public string? ValueText { get; set; }

    /// <summary>
    /// Product attribute type.
    /// </summary>
    public ProductAttributeType? ProductAttributeType { get; set; }
}
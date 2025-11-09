using VelocipedeUtils.Shared.Models.Business.BusinessDocuments;
using VelocipedeUtils.Shared.Models.Business.Products;

namespace VelocipedeUtils.Shared.Models.Business.Cooking;

/// <summary>
/// 
/// </summary>
public class InitialOrderIngredient : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Ingredient.
    /// </summary>
    public Ingredient? Ingredient { get; set; }
    
    /// <summary>
    /// Initial order.
    /// </summary>
    public InitialOrder? InitialOrder { get; set; }
    
    /// <summary>
    /// Initial order product.
    /// </summary>
    public InitialOrderProduct? InitialOrderProduct { get; set; }
    
    /// <summary>
    /// Quantity.
    /// </summary>
    public int Quantity { get; set; }
}
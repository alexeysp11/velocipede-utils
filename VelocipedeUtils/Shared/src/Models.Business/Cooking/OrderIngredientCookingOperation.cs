using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.Cooking;

/// <summary>
/// Associate table that connects a order ingredient and cooking operation.
/// </summary>
public class OrderIngredientCookingOperation : BusinessTask, IWfBusinessEntity
{
    /// <summary>
    /// Order ingredient.
    /// </summary>
    public OrderIngredient? OrderIngredient { get; set; }
    
    /// <summary>
    /// Cooking operation.
    /// </summary>
    public CookingOperation? CookingOperation { get; set; }
}
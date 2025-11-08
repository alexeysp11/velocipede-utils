using VelocipedeUtils.Shared.Models.Business.Processes;
using VelocipedeUtils.Shared.Models.Business.Products;

namespace VelocipedeUtils.Shared.Models.Business.Cooking;

/// <summary>
/// Associate table that connects a order product and cooking operation.
/// </summary>
public class OrderProductCookingOperation : BusinessTask, IWfBusinessEntity
{
    /// <summary>
    /// Order product.
    /// </summary>
    public OrderProduct? OrderProduct { get; set; }
    
    /// <summary>
    /// Cooking operation.
    /// </summary>
    public CookingOperation? CookingOperation { get; set; }
}
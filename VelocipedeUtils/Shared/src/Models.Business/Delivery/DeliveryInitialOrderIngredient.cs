using VelocipedeUtils.Shared.Models.Business.Cooking;
using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.Delivery;

public class DeliveryInitialOrderIngredient : BusinessTask, IWfBusinessEntity
{
    public InitialOrderIngredient? InitialOrderIngredient { get; set; }
    public DeliveryOperation? DeliveryOperation { get; set; }
}

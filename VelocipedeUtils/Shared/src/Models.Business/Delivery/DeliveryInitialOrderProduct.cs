using VelocipedeUtils.Shared.Models.Business.Processes;
using VelocipedeUtils.Shared.Models.Business.Products;

namespace VelocipedeUtils.Shared.Models.Business.Delivery;

public class DeliveryInitialOrderProduct : BusinessTask, IWfBusinessEntity
{
    public InitialOrderProduct? InitialOrderProduct { get; set; }
    public DeliveryOperation? DeliveryOperation { get; set; }
}

using VelocipedeUtils.Shared.Models.Business.BusinessDocuments;
using VelocipedeUtils.Shared.Models.Business.Processes;

namespace VelocipedeUtils.Shared.Models.Business.Delivery;

public class DeliveryInitialOrder : BusinessTask, IWfBusinessEntity
{
    public InitialOrder? InitialOrder { get; set; }
    public DeliveryOperation? DeliveryOperation { get; set; }
}

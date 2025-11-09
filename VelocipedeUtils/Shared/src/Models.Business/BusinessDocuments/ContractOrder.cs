namespace VelocipedeUtils.Shared.Models.Business.BusinessDocuments;

/// <summary>
/// Associate table between contract and order.
/// </summary>
public class ContractOrder : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Contract.
    /// </summary>
    public Contract? Contract { get; set; }

    /// <summary>
    /// Order.
    /// </summary>
    public Order? Order { get; set; }
}
namespace VelocipedeUtils.Shared.Models.Business;

/// <summary>
/// Interface that represents a business entity which could be received.
/// </summary>
public interface IReceivableBusinessEntity : IWfBusinessEntity
{
    /// <summary>
    /// Date the business entity was received.
    /// </summary>
    DateTime? DateReceived { get; set; }
}
namespace VelocipedeUtils.Shared.Models.Business;

/// <summary>
/// Interface that represents a business entity which could be sent.
/// </summary>
public interface ISendableBusinessEntity : IWfBusinessEntity
{
    /// <summary>
    /// Date the business entity was sent.
    /// </summary>
    DateTime? DateSent { get; set; }
}
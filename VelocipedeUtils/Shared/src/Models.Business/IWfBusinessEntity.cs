namespace VelocipedeUtils.Shared.Models.Business;

/// <summary>
/// Interface that represents a business entity.
/// </summary>
public interface IWfBusinessEntity
{
    /// <summary>
    /// ID of the business entity.
    /// </summary>
    long Id { get; set; }
    
    /// <summary>
    /// UID of the business entity.
    /// </summary>
    string? Uid { get; set; }
    
    /// <summary>
    /// Name of the business entity.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// Description of the business entity.
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// The date when the business entity was created.
    /// </summary>
    DateTime? DateCreated { get; set; }

    /// <summary>
    /// The date when the business entity was changed.
    /// </summary>
    DateTime? DateChanged { get; set; }

    /// <summary>
    /// Business entity status.
    /// </summary>
    BusinessEntityStatus? BusinessEntityStatus { get; set; }
}
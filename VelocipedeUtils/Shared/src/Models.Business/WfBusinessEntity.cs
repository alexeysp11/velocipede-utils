namespace VelocipedeUtils.Shared.Models.Business;

/// <summary>
/// Business entity in the velocipede-utils.
/// </summary>
public abstract class WfBusinessEntity : IWfBusinessEntity
{
    /// <summary>
    /// ID of the business entity.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// UID of the business entity.
    /// </summary>
    public string? Uid { get; set; }
    
    /// <summary>
    /// Name of the business entity.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the business entity.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The date when the business entity was created.
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// The date when the business entity was changed.
    /// </summary>
    public DateTime? DateChanged { get; set; }

    /// <summary>
    /// Business entity status.
    /// </summary>
    public BusinessEntityStatus? BusinessEntityStatus { get; set; }
}
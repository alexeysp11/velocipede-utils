namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// Physical location of the product in the warehouse.
/// It can have characteristics (e.g. size, load capacity, temperature conditions).
/// Allows you to effectively organize the placement and search of products.
/// </summary>
public class Location : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Number of the location.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Load capacity of the location.
    /// </summary>
    public int? LoadCapacity { get; set; }

    /// <summary>
    /// Location type.
    /// </summary>
    public LocationType? LocationType { get; set; }

    /// <summary>
    /// Zone.
    /// </summary>
    public Zone? Zone { get; set; }

    /// <summary>
    /// Determines whether the location is virtual.
    /// </summary>
    public bool? IsVirtual { get; set; }
}
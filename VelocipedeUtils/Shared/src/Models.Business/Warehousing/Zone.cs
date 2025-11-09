namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// A group of locations united by some feature (e.g. receiving zone, shipping zone, storage zone).
/// Allows you to group locations for ease of management, route optimization and safety.
/// </summary>
public class Zone : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Number of the location.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Zone type.
    /// </summary>
    public ZoneType? ZoneType { get; set; }
}
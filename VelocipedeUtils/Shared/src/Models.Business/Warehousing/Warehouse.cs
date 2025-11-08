using VelocipedeUtils.Shared.Models.Business.InformationSystem;

namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// Warehouse.
/// </summary>
public class Warehouse : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Organization item related to the warehouse.
    /// </summary>
    public required OrganizationItem OrganizationItem { get; set; }

    /// <summary>
    /// Warehouse items that are stored or related to the warehouse.
    /// </summary>
    public required ICollection<WarehouseItem> WarehouseItems { get; set; }

    /// <summary>
    /// Address of the warehouse.
    /// </summary>
    public string? Address { get; set; }
}
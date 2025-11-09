namespace VelocipedeUtils.Shared.Models.Business.Packing;

/// <summary>
/// A group of identical inventory items produced at the same time or to the same specification
/// (e.g. a batch of products with the same production date).
/// Simplifies product management, especially when tracking expiration dates or other production-related characteristics.
/// </summary>
public class Lot : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Code of the lot.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Description of the type of packaging (e.g. box, pallet, crate).
    /// </summary>
    public PackType? PackType { get; set; }
}
namespace VelocipedeUtils.Shared.Models.Business.Packing;

/// <summary>
/// A physical container in which a product is stored or moved (e.g. a box, pallet, barrel).
/// Can contain multiple units or batches of a product.
/// </summary>
public class Container : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Serial shipping container code.
    /// </summary>
    public string? Sscc { get; set; }

    /// <summary>
    /// Description of the type of packaging (e.g. box, pallet, crate).
    /// </summary>
    public PackType? PackType { get; set; }
}
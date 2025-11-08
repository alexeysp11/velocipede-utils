namespace VelocipedeUtils.Shared.Models.Business.Packing;

/// <summary>
/// A small movable container, often used to store and move individual items or small batches within a warehouse. 
/// May be of standardized size and used in conveyor systems.
/// </summary>
public class Tray : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Code of the tray.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Description of the type of packaging (e.g. box, pallet, crate).
    /// </summary>
    public PackType? PackType { get; set; }
}
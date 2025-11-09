using VelocipedeUtils.Shared.Models.Business.MeasurementUnits;

namespace VelocipedeUtils.Shared.Models.Business.Packing;

/// <summary>
/// Description of the type of packaging (e.g. box, pallet, crate).
/// Includes dimensions, weight, material, and other characteristics.
/// </summary>
public class PackType : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Code of the pack type.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Weight of the pack type.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Weight unit.
    /// </summary>
    public WeightUnit? WeightUnit { get; set; }

    /// <summary>
    /// Pack type material.
    /// </summary>
    public PackTypeMaterial? PackTypeMaterial { get; set; }
}
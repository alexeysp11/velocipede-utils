namespace VelocipedeUtils.Shared.Models.Business.MeasurementUnits;

/// <summary>
/// Weight unit.
/// </summary>
public class WeightUnit : WfBusinessEntity, IWfBusinessEntity
{
    /// <summary>
    /// Code of the weight unit.
    /// </summary>
    public string? Code { get; set; }
}
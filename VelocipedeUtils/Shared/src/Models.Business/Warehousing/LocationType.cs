namespace VelocipedeUtils.Shared.Models.Business.Warehousing;

/// <summary>
/// Definition of the functional purpose of the location (e.g. receiving location, storage location, picking location).
/// </summary>
public enum LocationType
{
    None = 0,
    Receiving = 1,
    Storage = 2,
    Picking = 3
}
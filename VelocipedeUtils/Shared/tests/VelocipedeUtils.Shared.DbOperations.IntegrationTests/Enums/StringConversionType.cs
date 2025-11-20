namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Enums;

/// <summary>
/// Conversion type for a string.
/// </summary>
/// <remarks>Could be used for converting string for case sensitive/insensitive tests.</remarks>
public enum StringConversionType
{
    /// <summary>
    /// String is not converted.
    /// </summary>
    None = 0,

    /// <summary>
    /// String is converted to lowercase.
    /// </summary>
    ToLower = 1,

    /// <summary>
    /// String is converted to uppercase.
    /// </summary>
    ToUpper = 2,
}

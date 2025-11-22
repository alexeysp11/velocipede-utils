namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Enums;

/// <summary>
/// Case conversion type for a string.
/// </summary>
/// <remarks>Could be used for converting string for case sensitive/insensitive tests.</remarks>
public enum CaseConversionType
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

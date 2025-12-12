namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Enums;

/// <summary>
/// Delimit identifier type.
/// </summary>
public enum DelimitIdentifierType
{
    /// <summary>
    /// No quotes used.
    /// </summary>
    None = 0,

    /// <summary>
    /// Used to delimit identifiers using square brackets, e.g. <c>[Field name]</c>.
    /// </summary>
    /// <remarks>Could be used in MS SQL, SQLite.</remarks>
    SquareBrackets = 1,

    /// <summary>
    /// Used to delimit identifiers using double quotes, e.g. <c>"Field name"</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Could be used in different databases, such as PostgreSQL, SQLite, Clickhouse, MS SQL.
    /// </para>
    /// <para>
    /// In MS SQL, can also be used to delimit identifiers, similar to square brackets,
    /// but their behavior is dependent on the <c>SET QUOTED_IDENTIFIER</c> setting.
    /// </para>
    /// </remarks>
    DoubleQuotes = 2,
}

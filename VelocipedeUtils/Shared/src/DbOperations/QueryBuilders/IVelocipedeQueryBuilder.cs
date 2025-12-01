using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders;

/// <summary>
/// Query builder interface.
/// </summary>
public interface IVelocipedeQueryBuilder
{
    /// <summary>
    /// Database type.
    /// </summary>
    DatabaseType DatabaseType { get; }

    /// <summary>
    /// Create a new table using <see cref="ICreateTableQueryBuilder"/>.
    /// </summary>
    /// <param name="tableName">Table name, that could contain schema name.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder CreateTable(string tableName);
}

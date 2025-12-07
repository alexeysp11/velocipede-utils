using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders;

/// <summary>
/// Query builder interface.
/// </summary>
public interface IVelocipedeQueryBuilder
{
    /// <summary>
    /// Database type.
    /// </summary>
    VelocipedeDatabaseType DatabaseType { get; }

    /// <summary>
    /// Instance of <see cref="IVelocipedeDbConnection"/>.
    /// </summary>
    /// <remarks>Can be used to explicitly identify which <see cref="IVelocipedeDbConnection"/> instance this query builder belongs to.</remarks>
    IVelocipedeDbConnection? DbConnection { get; }

    /// <summary>
    /// Create a new table using <see cref="ICreateTableQueryBuilder"/>.
    /// </summary>
    /// <param name="tableName">Table name, that could contain schema name.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder CreateTable(string tableName);
}

using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders;

/// <summary>
/// Query builder.
/// </summary>
public sealed class VelocipedeQueryBuilder : IVelocipedeQueryBuilder
{
    /// <inheritdoc/>
    public DatabaseType DatabaseType { get; }

    /// <summary>
    /// Default constructor for creating instance of <see cref="VelocipedeQueryBuilder"/>.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    public VelocipedeQueryBuilder(DatabaseType databaseType)
    {
        VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);

        DatabaseType = databaseType;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder CreateTable(string tableName)
    {
        return new CreateTableQueryBuilder(DatabaseType, tableName);
    }
}

using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders;

/// <summary>
/// Query builder.
/// </summary>
public sealed class VelocipedeQueryBuilder : IVelocipedeQueryBuilder
{
    /// <inheritdoc/>
    public VelocipedeDatabaseType DatabaseType { get; }

    /// <inheritdoc/>
    public IVelocipedeDbConnection? DbConnection { get; }

    /// <summary>
    /// Default constructor for creating instance of <see cref="VelocipedeQueryBuilder"/>.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    public VelocipedeQueryBuilder(VelocipedeDatabaseType databaseType)
    {
        VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);

        DatabaseType = databaseType;
    }

    /// <summary>
    /// Constructor for creating instance of <see cref="VelocipedeQueryBuilder"/> by specifying <see cref="IVelocipedeDbConnection"/>.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    /// <param name="dbConnection">Instance of <see cref="IVelocipedeDbConnection"/>.</param>
    public VelocipedeQueryBuilder(VelocipedeDatabaseType databaseType, IVelocipedeDbConnection dbConnection)
        : this(databaseType)
    {
        DbConnection = dbConnection;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder CreateTable(string tableName)
    {
        return new CreateTableQueryBuilder(DatabaseType, tableName);
    }
}

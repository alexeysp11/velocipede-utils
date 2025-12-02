using System.Data;
using System.Text;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

/// <summary>
/// Query builder for <c>CREATE TABLE</c> statement.
/// </summary>
public sealed class CreateTableQueryBuilder : ICreateTableQueryBuilder
{
    /// <inheritdoc/>
    public DatabaseType DatabaseType { get; }

    /// <inheritdoc/>
    public string TableName { get; }

    /// <inheritdoc/>
    public IEnumerable<VelocipedeColumnInfo> ColumnInfos {  get; private set; }

    /// <inheritdoc/>
    public IEnumerable<VelocipedeForeignKeyInfo> ForeignKeyInfos { get; private set; }

    private bool _built;
    private StringBuilder? _query;

    /// <summary>
    /// Default constructor for creating <see cref="CreateTableQueryBuilder"/> object.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    /// <param name="tableName">Name of the created table.</param>
    public CreateTableQueryBuilder(DatabaseType databaseType, string tableName)
    {
        VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);
        ArgumentException.ThrowIfNullOrEmpty(tableName);

        DatabaseType = databaseType;
        TableName = tableName;
        ColumnInfos = [];
        ForeignKeyInfos = [];

        _built = false;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder Build()
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(_built);

        _query = new StringBuilder();
        _built = true;

        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithColumn(
        string columnName,
        DbType columnType,
        int? charMaxLength = null,
        int? numericPrecision = null,
        int? numericScale = null,
        object? defaultValue = null,
        bool isPrimaryKey = false,
        bool isNullable = true)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(_built);
        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithColumn(VelocipedeColumnInfo columnInfo)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(_built);
        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithColumns(IEnumerable<VelocipedeColumnInfo> columnInfoList)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(_built);
        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithForeignKey(VelocipedeForeignKeyInfo foreignKeyInfo)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(_built);
        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithForeignKeys(IEnumerable<VelocipedeForeignKeyInfo> foreignKeyInfoList)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(_built);
        return this;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (!_built)
        {
            Build();
        }
        string? result = _query?.ToString();
        if (string.IsNullOrEmpty(result))
        {
            throw new VelocipedeQueryBuilderException(ErrorMessageConstants.IncorrectQueryBuilderResult);
        }
        return result;
    }
}

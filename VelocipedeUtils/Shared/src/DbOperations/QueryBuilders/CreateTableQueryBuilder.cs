using System.Data;
using System.Text;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders;

/// <summary>
/// Query builder for <c>CREATE TABLE</c> statement.
/// </summary>
public sealed class CreateTableQueryBuilder : ICreateTableQueryBuilder
{
    /// <inheritdoc/>
    public VelocipedeDatabaseType DatabaseType { get; }

    /// <inheritdoc/>
    public string TableName { get; }

    /// <inheritdoc/>
    public List<VelocipedeColumnInfo> ColumnInfos {  get; private set; }

    /// <inheritdoc/>
    public List<VelocipedeForeignKeyInfo> ForeignKeyInfos { get; private set; }

    /// <inheritdoc/>
    public bool IsBuilt {  get; private set; }

    private StringBuilder? _query;

    /// <summary>
    /// Default constructor for creating <see cref="CreateTableQueryBuilder"/> object.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    /// <param name="tableName">Name of the created table.</param>
    public CreateTableQueryBuilder(VelocipedeDatabaseType databaseType, string tableName)
    {
        VelocipedeQueryBuilderException.ThrowIfIncorrectDatabaseType(databaseType);
        if (string.IsNullOrEmpty(tableName))
        {
            throw new VelocipedeTableNameException();
        }

        DatabaseType = databaseType;
        TableName = tableName;
        ColumnInfos = [];
        ForeignKeyInfos = [];

        IsBuilt = false;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder Build()
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(IsBuilt);

        _query = new StringBuilder();
        IsBuilt = true;

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
        VelocipedeQueryBuilderException.ThrowIfBuilt(IsBuilt);

        // Validate arguments.
        ArgumentException.ThrowIfNullOrEmpty(columnName);

        // Add new column.

        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithColumn(VelocipedeColumnInfo columnInfo)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(IsBuilt);
        ArgumentNullException.ThrowIfNull(columnInfo);

        ColumnInfos.Add(columnInfo);

        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithColumns(IEnumerable<VelocipedeColumnInfo> columnInfoList)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(IsBuilt);
        ArgumentNullException.ThrowIfNull(columnInfoList);
        if (!columnInfoList.Any())
        {
            throw new InvalidOperationException(ErrorMessageConstants.EmptyColumnInfoList);
        }

        ColumnInfos.AddRange(columnInfoList);

        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithForeignKey(VelocipedeForeignKeyInfo foreignKeyInfo)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(IsBuilt);
        ArgumentNullException.ThrowIfNull(foreignKeyInfo);

        ForeignKeyInfos.Add(foreignKeyInfo);

        return this;
    }

    /// <inheritdoc/>
    public ICreateTableQueryBuilder WithForeignKeys(IEnumerable<VelocipedeForeignKeyInfo> foreignKeyInfoList)
    {
        VelocipedeQueryBuilderException.ThrowIfBuilt(IsBuilt);
        ArgumentNullException.ThrowIfNull(foreignKeyInfoList);
        if (!foreignKeyInfoList.Any())
        {
            throw new InvalidOperationException(ErrorMessageConstants.EmptyForeignKeyInfoList);
        }

        ForeignKeyInfos.AddRange(foreignKeyInfoList);

        return this;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (!IsBuilt)
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

using System.Data;
using System.Text;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

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
        _query.AppendLine($"CREATE TABLE {TableName} (");
        for (int i = 0; i < ColumnInfos.Count; i++)
        {
            VelocipedeColumnInfo col = ColumnInfos[i];

            // Format a column: "ColumnName DBType NULL/NOT NULL DEFAULT Value".
            _query.Append($"    {col.ColumnName} {col.NativeColumnType}");

            if (!col.IsNullable)
            {
                _query.Append(" NOT NULL");
            }

            if (col.DefaultValue != null)
            {
                string formattedDefault = col.FormatDefaultValue();
                _query.Append($" DEFAULT {formattedDefault}");
            }

            // Separator.
            if (i < ColumnInfos.Count - 1 || ForeignKeyInfos.Count != 0)
            {
                _query.AppendLine(",");
            }
            else
            {
                _query.AppendLine();
            }
        }

        // 4. Adding primary key constraints (including if there are several of them).
        List<VelocipedeColumnInfo> primaryKeys = ColumnInfos.Where(c => c.IsPrimaryKey).ToList();
        if (primaryKeys.Count != 0)
        {
            string pkColumnNames = string.Join(", ", primaryKeys.Select(c => c.ColumnName));

            // CONSTRAINT PK_TableName PRIMARY KEY (Col1, Col2)
            _query.Append($"    CONSTRAINT PK_{TableName} PRIMARY KEY ({pkColumnNames})");

            // Separator if FK follows.
            if (ForeignKeyInfos.Count != 0)
            {
                _query.AppendLine(",");
            }
            else
            {
                _query.AppendLine();
            }
        }

        // 3. Add foreign key constraints.
        for (int i = 0; i < ForeignKeyInfos.Count; i++)
        {
            VelocipedeForeignKeyInfo fk = ForeignKeyInfos[i];

            // CONSTRAINT FK_TableName_ColumnName FOREIGN KEY (ColumnName) REFERENCES RefTable(RefColumn)
            _query.Append($"    CONSTRAINT FK_{TableName}_{fk.FromColumn} FOREIGN KEY ({fk.FromColumn}) REFERENCES {fk.ToTableName}({fk.ToColumn})");

            if (i < ForeignKeyInfos.Count - 1)
            {
                _query.AppendLine(",");
            }
            else
            {
                _query.AppendLine();
            }
        }

        // 5. Completing the request.
        _query.AppendLine(");");

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
        ArgumentException.ThrowIfNullOrEmpty(columnName);

        ColumnInfos.Add(new VelocipedeColumnInfo
        {
            DatabaseType = DatabaseType,
            ColumnName = columnName,
            ColumnType = columnType,
            CharMaxLength = charMaxLength,
            NumericPrecision = numericPrecision,
            NumericScale = numericScale,
            DefaultValue = defaultValue,
            IsPrimaryKey = isPrimaryKey,
            IsNullable = isNullable
        });

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

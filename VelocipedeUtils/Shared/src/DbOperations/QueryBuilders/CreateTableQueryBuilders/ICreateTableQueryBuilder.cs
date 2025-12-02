using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.QueryBuilders.CreateTableQueryBuilders;

/// <summary>
/// Query builder interface for <c>CREATE TABLE</c> statement.
/// </summary>
public interface ICreateTableQueryBuilder
{
    /// <summary>
    /// Database type.
    /// </summary>
    DatabaseType DatabaseType { get; }

    /// <summary>
    /// Name of the created table.
    /// </summary>
    string TableName { get; }

    /// <summary>
    /// Collection of metadata objects about table columns.
    /// </summary>
    IEnumerable<VelocipedeColumnInfo> ColumnInfos { get; }

    /// <summary>
    /// Collection of metadata objects about table FK.
    /// </summary>
    IEnumerable<VelocipedeForeignKeyInfo> ForeignKeyInfos { get; }

    /// <summary>
    /// Build the query.
    /// </summary>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder Build();

    /// <summary>
    /// Specify column that should be created.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="columnType">The type of the column.</param>
    /// <param name="charMaxLength">The declared maximum length of a character or bit string type.</param>
    /// <param name="numericPrecision">Numeric precision for integer/decimal/numeric.</param>
    /// <param name="numericScale">Numeric scale for integer/decimal/numeric.</param>
    /// <param name="defaultValue">Default value of the column.</param>
    /// <param name="isPrimaryKey">Whether the column is a primary key.</param>
    /// <param name="isNullable">Whether the column is nullable.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder WithColumn(
        string columnName,
        DbType columnType,
        int? charMaxLength = null,
        int? numericPrecision = null,
        int? numericScale = null,
        object? defaultValue = null,
        bool isPrimaryKey = false,
        bool isNullable = true);
    
    /// <summary>
    /// Specify column that should be created.
    /// </summary>
    /// <param name="columnInfo">Metadata about a table column.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder WithColumn(VelocipedeColumnInfo columnInfo);

    /// <summary>
    /// Specify a list of columns that should be created.
    /// </summary>
    /// <param name="columnInfoList">Metadata about table columns.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder WithColumns(IEnumerable<VelocipedeColumnInfo> columnInfoList);

    /// <summary>
    /// Specify the foreign key that should be created.
    /// </summary>
    /// <param name="foreignKeyInfo">Info about foreign key constraints.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder WithForeignKey(VelocipedeForeignKeyInfo foreignKeyInfo);

    /// <summary>
    /// Specify a list foreign keys that should be created.
    /// </summary>
    /// <param name="foreignKeyInfoList">List that contains info about foreign key constraints.</param>
    /// <returns>Instance of <see cref="ICreateTableQueryBuilder"/>.</returns>
    ICreateTableQueryBuilder WithForeignKeys(IEnumerable<VelocipedeForeignKeyInfo> foreignKeyInfoList);
}

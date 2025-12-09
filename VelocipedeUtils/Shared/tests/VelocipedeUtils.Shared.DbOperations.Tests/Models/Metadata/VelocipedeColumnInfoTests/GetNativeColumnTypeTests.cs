using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models.Metadata.VelocipedeColumnInfoTests;

/// <summary>
/// Unit tests for <see cref="VelocipedeColumnInfoExtensions.GetNativeType"/>.
/// </summary>
public sealed class GetNativeColumnTypeTests
{
    private const string COLUMN_NAME = "ColumnName";

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoInteger()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Byte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Int16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.UInt16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Int32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.UInt32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Int64 }, ExpectedNativeType = "bigint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.UInt64 }, ExpectedNativeType = "bigint" } },
            
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.SByte }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Byte }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Int16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.UInt16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Int32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.UInt32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Int64 }, ExpectedNativeType = "bigint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.UInt64 }, ExpectedNativeType = "bigint" } },
            
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Byte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Int16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.UInt16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Int32 }, ExpectedNativeType = "int" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.UInt32 }, ExpectedNativeType = "int" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Int64 }, ExpectedNativeType = "bigint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.UInt64 }, ExpectedNativeType = "bigint" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoText()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = null }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = null }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = null }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = -1 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = -1 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = -50 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = -50 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = 50 }, ExpectedNativeType = "nvarchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "nvarchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoBlob()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Binary }, ExpectedNativeType = "blob" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Binary }, ExpectedNativeType = "bytea" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Binary }, ExpectedNativeType = "binary" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoReal()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Double }, ExpectedNativeType = "double precision" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Single }, ExpectedNativeType = "double precision" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Double }, ExpectedNativeType = "double precision" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Single }, ExpectedNativeType = "double precision" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Double }, ExpectedNativeType = "double precision" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Single }, ExpectedNativeType = "double precision" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoNumeric()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = "numeric(10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = "decimal(10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = "numeric(10, 5)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = "decimal(10, 5)" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = "numeric(10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = "decimal(10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = "numeric(10, 5)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = "decimal(10, 5)" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = "numeric" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = "decimal" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = "numeric(10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = "decimal(10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = "numeric(10, 5)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = "decimal(10, 5)" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoBoolean()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Boolean }, ExpectedNativeType = "boolean" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Boolean }, ExpectedNativeType = "boolean" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Boolean }, ExpectedNativeType = "bit" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoDatetime()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Time }, ExpectedNativeType = "time" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Date }, ExpectedNativeType = "date" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.DateTime }, ExpectedNativeType = "datetime" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.DateTime2 }, ExpectedNativeType = "datetime2" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.DateTimeOffset }, ExpectedNativeType = "datetime" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Time }, ExpectedNativeType = "time" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Date }, ExpectedNativeType = "date" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.DateTime }, ExpectedNativeType = "timestamp" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.DateTime2 }, ExpectedNativeType = "timestamp" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.DateTimeOffset }, ExpectedNativeType = "timestamp with time zone" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Time }, ExpectedNativeType = "time" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Date }, ExpectedNativeType = "date" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.DateTime }, ExpectedNativeType = "datetime" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.DateTime2 }, ExpectedNativeType = "datetime2" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.DateTimeOffset }, ExpectedNativeType = "datetimeoffset" } },
        };
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoInteger))]
    [MemberData(nameof(GetColumnInfoText))]
    [MemberData(nameof(GetColumnInfoBlob))]
    [MemberData(nameof(GetColumnInfoReal))]
    [MemberData(nameof(GetColumnInfoNumeric))]
    [MemberData(nameof(GetColumnInfoBoolean))]
    [MemberData(nameof(GetColumnInfoDatetime))]
    public void GetNativeColumnType(TestCaseNativeColumnType testCase)
    {
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        columnInfo.NativeColumnType
            .Should()
            .Be(testCase.ExpectedNativeType);
    }
}

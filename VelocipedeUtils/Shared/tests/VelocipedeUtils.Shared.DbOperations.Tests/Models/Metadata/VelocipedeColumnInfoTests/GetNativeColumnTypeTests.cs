using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models.Metadata.VelocipedeColumnInfoTests;

/// <summary>
/// Unit tests for <see cref="VelocipedeColumnInfoExtensions.GetNativeType"/>.
/// </summary>
public sealed class GetNativeColumnTypeTests
{
    private const string COLUMN_NAME = "ColumnName";

    #region Test cases
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
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = "numeric(10, 10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = "decimal(10, 10)" } },

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
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = "numeric(10, 10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = "decimal(10, 10)" } },

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
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = "numeric(10, 10)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = "decimal(10, 10)" } },
        };
    }
    
    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoCurrency()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 2 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 4 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = 2 }, ExpectedNativeType = $"numeric(2, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = 2 }, ExpectedNativeType = $"numeric(4, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = 4 }, ExpectedNativeType = $"numeric(4, 4)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = null }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = -1 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 0 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 2 }, ExpectedNativeType = $"numeric(5, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 4 }, ExpectedNativeType = $"numeric(5, 4)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 5 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = null }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = -1 }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = 15 }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 2 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 4 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = 2 }, ExpectedNativeType = $"numeric(2, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = 2 }, ExpectedNativeType = $"numeric(4, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = 4 }, ExpectedNativeType = $"numeric(4, 4)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = null }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = -1 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 0 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 2 }, ExpectedNativeType = $"numeric(5, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 4 }, ExpectedNativeType = $"numeric(5, 4)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 5 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = null }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = -1 }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = 15 }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 0 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 2 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 4 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = 0 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 2, NumericScale = 2 }, ExpectedNativeType = $"numeric(2, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = null }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = -1 }, ExpectedNativeType = $"numeric({NumericConstants.CurrencyDefaultPrecision}, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = 2 }, ExpectedNativeType = $"numeric(4, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 4, NumericScale = 4 }, ExpectedNativeType = $"numeric(4, 4)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = null }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = -1 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 0 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 2 }, ExpectedNativeType = $"numeric(5, 2)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 4 }, ExpectedNativeType = $"numeric(5, 4)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 5 }, ExpectedNativeType = $"numeric(5, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = null }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 5 }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 10 }, ExpectedNativeType = $"numeric(10, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = null }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = -1 }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Currency, NumericPrecision = 15, NumericScale = 15 }, ExpectedNativeType = $"numeric(15, {NumericConstants.CurrencyDefaultScale})" } },
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

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoIncorrectNumerics(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = 1, NumericScale = 2 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = 5, NumericScale = 10 },

        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = 1, NumericScale = 2 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = 5, NumericScale = 10 },

        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 1, NumericScale = 2 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 5, NumericScale = 10 },
    ];

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoGuid()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Guid }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Guid }, ExpectedNativeType = "uuid" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Guid }, ExpectedNativeType = "uniqueidentifier" } },
        };
    }
    #endregion  // Test cases

    [Theory]
    [MemberData(nameof(GetColumnInfoInteger))]
    [MemberData(nameof(GetColumnInfoText))]
    [MemberData(nameof(GetColumnInfoBlob))]
    [MemberData(nameof(GetColumnInfoReal))]
    [MemberData(nameof(GetColumnInfoNumeric))]
    [MemberData(nameof(GetColumnInfoCurrency))]
    [MemberData(nameof(GetColumnInfoBoolean))]
    [MemberData(nameof(GetColumnInfoDatetime))]
    [MemberData(nameof(GetColumnInfoGuid))]
    public void GetNativeColumnType(TestCaseNativeColumnType testCase)
    {
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        columnInfo.NativeColumnType
            .Should()
            .Be(testCase.ExpectedNativeType);
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoNumeric))]
    [MemberData(nameof(GetColumnInfoCurrency))]
    public void GetNumericNativeColumnType_NumericAndBaseTypeIsCorrect(TestCaseNativeColumnType testCase)
    {
        // Arrange.
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        string baseType = columnInfo.ColumnType is DbType.VarNumeric or DbType.Currency ? "numeric" : "decimal";

        // Act.
        string result = columnInfo.GetNumericNativeColumnType(baseType);

        // Assert.
        result
            .Should()
            .Be(testCase.ExpectedNativeType);
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoIncorrectNumerics), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoIncorrectNumerics), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoIncorrectNumerics), parameters: VelocipedeDatabaseType.MSSQL)]
    public void GetNumericNativeColumnType_IncorrectNumerics(VelocipedeColumnInfo columnInfo)
    {
        // Arrange.
        string baseType = columnInfo.ColumnType is DbType.VarNumeric or DbType.Currency ? "numeric" : "decimal";
        Func<string> act = () => columnInfo.GetNumericNativeColumnType(baseType);

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.NumericScaleBiggerThanPrecision);
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoNumeric))]
    [MemberData(nameof(GetColumnInfoCurrency))]
    public void GetNumericNativeColumnType_NumericAndBaseTypeIsIncorrect(TestCaseNativeColumnType testCase)
    {
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        Func<string> act = () => columnInfo.GetNumericNativeColumnType("not-numeric-or-decimal");
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.IncorrectBaseForNumericNativeColumnType);
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoInteger))]
    [MemberData(nameof(GetColumnInfoText))]
    [MemberData(nameof(GetColumnInfoBlob))]
    [MemberData(nameof(GetColumnInfoReal))]
    [MemberData(nameof(GetColumnInfoBoolean))]
    [MemberData(nameof(GetColumnInfoDatetime))]
    public void GetNumericNativeColumnType_NotNumericAndBaseTypeIsIncorrect(TestCaseNativeColumnType testCase)
    {
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        Func<string> act = () => columnInfo.GetNumericNativeColumnType("not-numeric-or-decimal");
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.NumericNativeColumnTypeConversion);
    }
}

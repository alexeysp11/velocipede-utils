using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models.Metadata;

/// <summary>
/// Unit tests for <see cref="VelocipedeColumnInfo"/>.
/// </summary>
public sealed class VelocipedeColumnInfoTests
{
    private const string COLUMN_NAME = "ColumnName";

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoInteger()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.Byte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.Int16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.UInt16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.Int32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.UInt32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.Int64 }, ExpectedNativeType = "bigint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.UInt64 }, ExpectedNativeType = "bigint" } },
            
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.SByte }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.Byte }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.Int16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.UInt16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.Int32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.UInt32 }, ExpectedNativeType = "integer" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.Int64 }, ExpectedNativeType = "bigint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.UInt64 }, ExpectedNativeType = "bigint" } },
            
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.Byte }, ExpectedNativeType = "tinyint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.Int16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.UInt16 }, ExpectedNativeType = "smallint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.Int32 }, ExpectedNativeType = "int" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.UInt32 }, ExpectedNativeType = "int" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.Int64 }, ExpectedNativeType = "bigint" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.UInt64 }, ExpectedNativeType = "bigint" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoText()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.String, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.StringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiString, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiStringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.String, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.StringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiString, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiStringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.String, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.StringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiString, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiStringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.String, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.StringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiString, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.AnsiStringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.String, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.StringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiString, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = null }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.String, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.StringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiString, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.String, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.StringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiString, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "text" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.String, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.StringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiString, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.String, CharMaxLength = null }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.StringFixedLength, CharMaxLength = null }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiString, CharMaxLength = null }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = null }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.String, CharMaxLength = -1 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.StringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiString, CharMaxLength = -1 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = -1 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.String, CharMaxLength = -50 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.StringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "nvarchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiString, CharMaxLength = -50 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = -50 }, ExpectedNativeType = "varchar(max)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.String, CharMaxLength = 50 }, ExpectedNativeType = "nvarchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.StringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "nvarchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiString, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.AnsiStringFixedLength, CharMaxLength = 50 }, ExpectedNativeType = "varchar(50)" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoBlob()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.Binary }, ExpectedNativeType = "blob" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.Binary }, ExpectedNativeType = "bytea" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.Binary }, ExpectedNativeType = "binary" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoReal()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoNumeric()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoBoolean()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.Boolean }, ExpectedNativeType = "boolean" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, DbType = DbType.Boolean }, ExpectedNativeType = "boolean" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, DbType = DbType.Boolean }, ExpectedNativeType = "bit" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoDatetime()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
        };
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoInteger))]
    [MemberData(nameof(GetColumnInfoText))]
    [MemberData(nameof(GetColumnInfoBlob))]
    [MemberData(nameof(GetColumnInfoReal), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoNumeric), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoBoolean))]
    [MemberData(nameof(GetColumnInfoDatetime), Skip = "Test is not implemented yet")]
    public void GetNativeColumnType(TestCaseNativeColumnType testCase)
    {
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        columnInfo.NativeColumnType
            .Should()
            .Be(testCase.ExpectedNativeType);
    }
}

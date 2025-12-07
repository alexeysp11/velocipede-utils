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
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoText()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
        };
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoBlob()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
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
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, DbType = DbType.SByte }, ExpectedNativeType = "tinyint" } },
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
    [MemberData(nameof(GetColumnInfoText), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoBlob), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoReal), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoNumeric), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoBoolean), Skip = "Test is not implemented yet")]
    [MemberData(nameof(GetColumnInfoDatetime), Skip = "Test is not implemented yet")]
    public void GetNativeColumnType(TestCaseNativeColumnType testCase)
    {
        VelocipedeColumnInfo columnInfo = testCase.ColumnInfo;
        columnInfo.NativeColumnType
            .Should()
            .Be(testCase.ExpectedNativeType);
    }
}

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

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoInteger()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
            { VelocipedeDatabaseType.SQLite, DbType.Byte, "tinyint" },
            { VelocipedeDatabaseType.SQLite, DbType.Int16, "smallint" },
            { VelocipedeDatabaseType.SQLite, DbType.UInt16, "smallint" },
            { VelocipedeDatabaseType.SQLite, DbType.Int32, "integer" },
            { VelocipedeDatabaseType.SQLite, DbType.UInt32, "integer" },
            { VelocipedeDatabaseType.SQLite, DbType.Int64, "bigint" },
            { VelocipedeDatabaseType.SQLite, DbType.UInt64, "bigint" },
        };
    }

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoText()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
        };
    }

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoBlob()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
        };
    }

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoReal()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
        };
    }

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoNumeric()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
        };
    }

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoBoolean()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
        };
    }

    public static TheoryData<VelocipedeDatabaseType, DbType, string> GetColumnInfoDatetime()
    {
        return new TheoryData<VelocipedeDatabaseType, DbType, string>
        {
            { VelocipedeDatabaseType.SQLite, DbType.SByte, "tinyint" },
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
    public void GetNativeColumnType(VelocipedeDatabaseType databaseType, DbType columnType, string expectedNativeType)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            ColumnName = COLUMN_NAME,
            DatabaseType = databaseType,
            DbType = columnType,
        };
        columnInfo.NativeColumnType.Should().Be(expectedNativeType);
    }
}

using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models.Metadata.VelocipedeColumnInfoTests;

/// <summary>
/// Unit tests for <see cref="VelocipedeColumnInfoExtensions.FormatDefaultValue"/>.
/// </summary>
public sealed class FormatDefaultValueTests
{
    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_StringValue_ShouldReturnQuotedString(VelocipedeDatabaseType dbType)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.String,
            DefaultValue = "test_default_value"
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("'test_default_value'");
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_StringValueWithQuote_ShouldReturnQuotedAndEscapedString(VelocipedeDatabaseType dbType)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.String,
            DefaultValue = "value_with_'quote'"
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("'value_with_''quote'''");
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite, DbType.Int32, 123)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, DbType.Decimal, 456.78)]
    [InlineData(VelocipedeDatabaseType.MSSQL, DbType.Double, 99.9)]
    [InlineData(VelocipedeDatabaseType.SQLite, DbType.Int64, 10000000000)]
    public void FormatDefaultValue_NumericTypes_ShouldReturnNonQuotedValue(
        VelocipedeDatabaseType dbType,
        DbType sqlDbType,
        object defaultValue)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = sqlDbType,
            DefaultValue = defaultValue
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be(defaultValue.ToString());
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite, true, "1")]
    [InlineData(VelocipedeDatabaseType.SQLite, false, "0")]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, true, "true")]
    [InlineData(VelocipedeDatabaseType.PostgreSQL, false, "false")]
    [InlineData(VelocipedeDatabaseType.MSSQL, true, "1")]
    [InlineData(VelocipedeDatabaseType.MSSQL, false, "0")]
    public void FormatDefaultValue_Boolean(
        VelocipedeDatabaseType databaseType,
        object defaultValue,
        string expected)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = databaseType,
            ColumnName = "TestColumn",
            ColumnType = DbType.Boolean,
            DefaultValue = defaultValue
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_DateTime_ShouldReturnQuotedIsoFormat(VelocipedeDatabaseType dbType)
    {
        DateTime testDateTime = new(2023, 10, 27, 15, 30, 0);
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.DateTime,
            DefaultValue = testDateTime
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("'2023-10-27 15:30:00'");
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_Date_ShouldReturnQuotedIsoFormat(VelocipedeDatabaseType dbType)
    {
        DateTime testDate = new(2023, 10, 27);
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.Date,
            DefaultValue = testDate
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("'2023-10-27 00:00:00'");
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_NullValue_ShouldReturnNullString(VelocipedeDatabaseType dbType)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.String,
            DefaultValue = null
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("NULL");
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_DBNullValue_ShouldReturnNullString(VelocipedeDatabaseType dbType)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.Int32,
            DefaultValue = DBNull.Value
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("NULL");
    }
}

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
    #region Test cases
    public static TheoryData<TestCaseFormatDefaultValue> GetTestCaseFormatDefaultNumerics(VelocipedeDatabaseType databaseType) => [
        // Positive integers.
        new() { DatabaseType = databaseType, ColumnType = DbType.SByte, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Byte, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int16, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt16, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int32, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt32, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int64, DefaultValue = 100, Expected = "100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt64, DefaultValue = 100, Expected = "100", },

        // Positive floating point numbers.
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = 123, Expected = "123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = 123, Expected = "123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = 123, Expected = "123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = 123, Expected = "123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = 123, Expected = "123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = 99.9m, Expected = "99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = 99.9m, Expected = "99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = 99.9m, Expected = "99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = 99.9, Expected = "99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = 99.9m, Expected = "99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = 299.92m, Expected = "299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = 299.92m, Expected = "299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = 299.92m, Expected = "299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = 299.92, Expected = "299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = 299.92m, Expected = "299.92", },

        // Negative integers.
        new() { DatabaseType = databaseType, ColumnType = DbType.SByte, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Byte, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int16, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt16, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int32, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt32, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int64, DefaultValue = -100, Expected = "-100", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt64, DefaultValue = -100, Expected = "-100", },
        
        // Positive floating point numbers.
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = -123, Expected = "-123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = -123, Expected = "-123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = -123, Expected = "-123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = -123, Expected = "-123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = -123, Expected = "-123", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = -99.9m, Expected = "-99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = -99.9m, Expected = "-99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = -99.9m, Expected = "-99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = -99.9m, Expected = "-99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = -99.9m, Expected = "-99.9", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = -299.92m, Expected = "-299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = -299.92m, Expected = "-299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = -299.92m, Expected = "-299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = -299.92m, Expected = "-299.92", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = -299.92m, Expected = "-299.92", },

        // Equals to 0.
        new() { DatabaseType = databaseType, ColumnType = DbType.SByte, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Byte, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int16, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt16, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int32, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt32, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int64, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt64, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = 0, Expected = "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = 0, Expected = "0", },

        // Equals to null.
        new() { DatabaseType = databaseType, ColumnType = DbType.SByte, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Byte, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int16, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt16, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int32, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt32, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int64, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt64, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = null, Expected = "NULL", },

        // Equals to DBNull.
        new() { DatabaseType = databaseType, ColumnType = DbType.SByte, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Byte, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int16, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt16, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int32, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt32, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Int64, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.UInt64, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Currency, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.VarNumeric, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Decimal, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Double, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Single, DefaultValue = DBNull.Value, Expected = "NULL", },
    ];
    #endregion  // Test cases

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_NullStringValue_ReturnsNull(VelocipedeDatabaseType dbType)
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
            ColumnType = DbType.String,
            DefaultValue = DBNull.Value
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("NULL");
    }

    [Theory]
    [InlineData(VelocipedeDatabaseType.SQLite)]
    [InlineData(VelocipedeDatabaseType.PostgreSQL)]
    [InlineData(VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_EmptyStringValue(VelocipedeDatabaseType dbType)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = dbType,
            ColumnName = "TestColumn",
            ColumnType = DbType.String,
            DefaultValue = string.Empty
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be("''");
    }

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
    [MemberData(nameof(GetTestCaseFormatDefaultNumerics), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCaseFormatDefaultNumerics), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCaseFormatDefaultNumerics), parameters: VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_NumericTypes(TestCaseFormatDefaultValue testCase)
    {
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = testCase.DatabaseType,
            ColumnName = "TestColumn",
            ColumnType = testCase.ColumnType,
            DefaultValue = testCase.DefaultValue,
        };

        string result = columnInfo.FormatDefaultValue();

        result.Should().Be(testCase.Expected);
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
}

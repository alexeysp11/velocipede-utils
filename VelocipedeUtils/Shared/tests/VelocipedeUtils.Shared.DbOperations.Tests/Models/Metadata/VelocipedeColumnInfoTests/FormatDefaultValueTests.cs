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
    private const string DEFAULT_GUID = "b42410ee-132f-42ee-9e4f-09a6485c95b8";

    private const string DEFAULT_XML_DOCUMENT = "<?xml version=\"1.0\"?><book><title>Manual</title><chapter>1</chapter></book>";
    private const string DEFAULT_XML_CONTENT = "<bar/><foo>abc</foo>";
    private const string DEFAULT_XML_COMMENT = "<!--hello-->";

    #region Test cases
    public static TheoryData<TestCaseFormatDefaultValue> GetTestCaseFormatDefaultStrings(VelocipedeDatabaseType databaseType) => [
        // String storing positive numerics.
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "100", Expected = "'100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, DefaultValue = "100", Expected = "'100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiString, DefaultValue = "100", Expected = "'100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, DefaultValue = "100", Expected = "'100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "100.28", Expected = "'100.28'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, DefaultValue = "100.28", Expected = "'100.28'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiString, DefaultValue = "100.28", Expected = "'100.28'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, DefaultValue = "100.28", Expected = "'100.28'", },

        // String storing negative numerics.
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "-100", Expected = "'-100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, DefaultValue = "-100", Expected = "'-100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiString, DefaultValue = "-100", Expected = "'-100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, DefaultValue = "-100", Expected = "'-100'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "-100.28", Expected = "'-100.28'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, DefaultValue = "-100.28", Expected = "'-100.28'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiString, DefaultValue = "-100.28", Expected = "'-100.28'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, DefaultValue = "-100.28", Expected = "'-100.28'", },

        // Quotes.
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "test_default_value", Expected = "'test_default_value'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "test_default_value 123", Expected = "'test_default_value 123'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "test_default_value ' 123", Expected = "'test_default_value '' 123'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "test_default_value '123/12", Expected = "'test_default_value ''123/12'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "test_default_value'123.2", Expected = "'test_default_value''123.2'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "value_with_'quote", Expected = "'value_with_''quote'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "value_with_'quote'", Expected = "'value_with_''quote'''", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "'value_with_'quote'", Expected = "'''value_with_''quote'''", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "'value_with_''quote'", Expected = "'''value_with_''''quote'''", },
        
        // Empty string.
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = "", Expected = "''", },
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = string.Empty, Expected = "''", },

        // Equals to null.
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiString, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, DefaultValue = null, Expected = "NULL", },

        // Equals to DBNull.
        new() { DatabaseType = databaseType, ColumnType = DbType.String, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiString, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, DefaultValue = DBNull.Value, Expected = "NULL", },
    ];

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
    
    public static TheoryData<TestCaseFormatDefaultValue> GetTestCaseFormatDefaultBoolean(VelocipedeDatabaseType databaseType) => [
        new() { DatabaseType = databaseType, ColumnType = DbType.Boolean, DefaultValue = true, Expected = databaseType is VelocipedeDatabaseType.PostgreSQL ? "true" : "1", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Boolean, DefaultValue = false, Expected = databaseType is VelocipedeDatabaseType.PostgreSQL ? "false" : "0", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Boolean, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Boolean, DefaultValue = DBNull.Value, Expected = "NULL", },
    ];

    public static TheoryData<TestCaseFormatDefaultValue> GetTestCaseFormatDefaultDateTime(VelocipedeDatabaseType databaseType) => [
        // Date and time.
        new() { DatabaseType = databaseType, ColumnType = DbType.Time, DefaultValue = new DateTime(2023, 10, 27, 15, 30, 0), Expected = "'15:30:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Date, DefaultValue = new DateTime(2023, 10, 27, 15, 30, 0), Expected = "'2023-10-27 15:30:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime, DefaultValue = new DateTime(2023, 10, 27, 15, 30, 0), Expected = "'2023-10-27 15:30:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime2, DefaultValue = new DateTime(2023, 10, 27, 15, 30, 0), Expected = "'2023-10-27 15:30:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTimeOffset, DefaultValue = new DateTime(2023, 10, 27, 15, 30, 0), Expected = "'2023-10-27 15:30:00'", },
        
        // Only date.
        new() { DatabaseType = databaseType, ColumnType = DbType.Time, DefaultValue = new DateTime(2023, 10, 27), Expected = "'00:00:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Date, DefaultValue = new DateTime(2023, 10, 27), Expected = "'2023-10-27 00:00:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime, DefaultValue = new DateTime(2023, 10, 27), Expected = "'2023-10-27 00:00:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime2, DefaultValue = new DateTime(2023, 10, 27), Expected = "'2023-10-27 00:00:00'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTimeOffset, DefaultValue = new DateTime(2023, 10, 27), Expected = "'2023-10-27 00:00:00'", },

        // Equals to null.
        new() { DatabaseType = databaseType, ColumnType = DbType.Time, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Date, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime2, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTimeOffset, DefaultValue = null, Expected = "NULL", },

        // Equals to DBNull.
        new() { DatabaseType = databaseType, ColumnType = DbType.Time, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Date, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTime2, DefaultValue = DBNull.Value, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.DateTimeOffset, DefaultValue = DBNull.Value, Expected = "NULL", },
    ];

    public static TheoryData<TestCaseFormatDefaultValue> GetTestCaseFormatDefaultGuid(VelocipedeDatabaseType databaseType) => [
        new() { DatabaseType = databaseType, ColumnType = DbType.Guid, DefaultValue = DEFAULT_GUID, Expected = $"'{DEFAULT_GUID}'", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Guid, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = databaseType, ColumnType = DbType.Guid, DefaultValue = DBNull.Value, Expected = "NULL", },
    ];

    public static TheoryData<TestCaseFormatDefaultValue> TestCaseFormatDefaultXml => [
        new() { DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_DOCUMENT, Expected = $"'{DEFAULT_XML_DOCUMENT}'", },
        new() { DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_CONTENT, Expected = $"'{DEFAULT_XML_CONTENT}'", },
        new() { DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_COMMENT, Expected = $"'{DEFAULT_XML_COMMENT}'", },
        new() { DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Xml, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Xml, DefaultValue = DBNull.Value, Expected = "NULL", },

        new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_DOCUMENT, Expected = $"'{DEFAULT_XML_DOCUMENT}'::xml", },
        new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_CONTENT, Expected = $"'{DEFAULT_XML_CONTENT}'::xml", },
        new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_COMMENT, Expected = $"'{DEFAULT_XML_COMMENT}'::xml", },
        new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Xml, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Xml, DefaultValue = DBNull.Value, Expected = "NULL", },

        new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_DOCUMENT, Expected = $"'{DEFAULT_XML_DOCUMENT}'", },
        new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_CONTENT, Expected = $"'{DEFAULT_XML_CONTENT}'", },
        new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Xml, DefaultValue = DEFAULT_XML_COMMENT, Expected = $"'{DEFAULT_XML_COMMENT}'", },
        new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Xml, DefaultValue = null, Expected = "NULL", },
        new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Xml, DefaultValue = DBNull.Value, Expected = "NULL", },
    ];
    #endregion  // Test cases

    [Theory]
    [MemberData(nameof(GetTestCaseFormatDefaultStrings), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCaseFormatDefaultStrings), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCaseFormatDefaultStrings), parameters: VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_Strings(TestCaseFormatDefaultValue testCase)
        => ValidateFormatDefaultValue(testCase);

    [Theory]
    [MemberData(nameof(GetTestCaseFormatDefaultNumerics), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCaseFormatDefaultNumerics), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCaseFormatDefaultNumerics), parameters: VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_Numerics(TestCaseFormatDefaultValue testCase)
        => ValidateFormatDefaultValue(testCase);

    [Theory]
    [MemberData(nameof(GetTestCaseFormatDefaultBoolean), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCaseFormatDefaultBoolean), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCaseFormatDefaultBoolean), parameters: VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_Boolean(TestCaseFormatDefaultValue testCase)
        => ValidateFormatDefaultValue(testCase);
    
    [Theory]
    [MemberData(nameof(GetTestCaseFormatDefaultDateTime), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCaseFormatDefaultDateTime), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCaseFormatDefaultDateTime), parameters: VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_DateTime(TestCaseFormatDefaultValue testCase)
        => ValidateFormatDefaultValue(testCase);

    [Theory]
    [MemberData(nameof(GetTestCaseFormatDefaultGuid), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetTestCaseFormatDefaultGuid), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetTestCaseFormatDefaultGuid), parameters: VelocipedeDatabaseType.MSSQL)]
    public void FormatDefaultValue_Guid(TestCaseFormatDefaultValue testCase)
        => ValidateFormatDefaultValue(testCase);

    [Theory]
    [MemberData(nameof(TestCaseFormatDefaultXml))]
    public void FormatDefaultValue_Xml(TestCaseFormatDefaultValue testCase)
        => ValidateFormatDefaultValue(testCase);

    private static void ValidateFormatDefaultValue(TestCaseFormatDefaultValue testCase)
    {
        // Arrange.
        VelocipedeColumnInfo columnInfo = new()
        {
            DatabaseType = testCase.DatabaseType,
            ColumnName = "TestColumn",
            ColumnType = testCase.ColumnType,
            DefaultValue = testCase.DefaultValue,
        };

        // Act.
        string result = columnInfo.FormatDefaultValue();

        // Assert.
        result
            .Should()
            .Be(testCase.Expected);
    }
}

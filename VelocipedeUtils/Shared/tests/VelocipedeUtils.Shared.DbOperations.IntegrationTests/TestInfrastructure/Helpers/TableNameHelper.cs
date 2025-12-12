using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Enums;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Helpers;

/// <summary>
/// Table name helper.
/// </summary>
public static class TableNameHelper
{
    /// <summary>
    /// Using the name of the test method, get the name with which the table will be created in the database.
    /// </summary>
    /// <remarks>
    /// If test method name is <c>MethodName</c>, conversion type is <see cref="CaseConversionType.ToLower"/>,
    /// and delimit identifier is <see cref="DelimitIdentifierType.DoubleQuotes"/>,
    /// then this method could return <c>MethodName_1_2</c> as a result.</remarks>
    /// <param name="methodName">Method name.</param>
    /// <param name="conversionType">Table name conversion type.</param>
    /// <param name="delimitIdentifierType">Delimit identifier type.</param>
    /// <returns>The name with which the table will be created in the database.</returns>
    public static string GetTableNameByTestMethod(
        string methodName,
        CaseConversionType conversionType = CaseConversionType.None,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        return $"{methodName}_{(int)conversionType}_{(int)delimitIdentifierType}";
    }

    /// <summary>
    /// Apply conversion to table name.
    /// </summary>
    /// <param name="tableName">Original table name.</param>
    /// <param name="databaseType">Database type.</param>
    /// <param name="conversionType">Table name conversion type.</param>
    /// <param name="delimitIdentifierType">Delimit identifier type.</param>
    /// <returns>New table name.</returns>
    public static string ConvertTableName(
        string tableName,
        VelocipedeDatabaseType databaseType,
        CaseConversionType conversionType = CaseConversionType.None,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        string result = conversionType switch
        {
            CaseConversionType.ToLower => tableName.ToLower(),
            CaseConversionType.ToUpper => tableName.ToUpper(),
            _ => tableName,
        };
        result = delimitIdentifierType switch
        {
            DelimitIdentifierType.SquareBrackets => databaseType == VelocipedeDatabaseType.PostgreSQL ? result : $"[{result}]",
            DelimitIdentifierType.DoubleQuotes => $@"""{result}""",
            _ => result,
        };
        return result;
    }
}

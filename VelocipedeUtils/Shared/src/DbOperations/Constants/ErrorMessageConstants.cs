namespace VelocipedeUtils.Shared.DbOperations.Constants;

/// <summary>
/// Error message constants.
/// </summary>
public static class ErrorMessageConstants
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public const string ConnectionStringShouldNotBeNullOrEmpty = "Connection string should not be null or empty";
    public const string DatabaseAlreadyExists = "Database already exists";
    public const string UnableToCreateDatabase = "Unable to create database";
    public const string UnableToConnectToDatabase = "Unable to connect to database";
    public const string IncorrectDatabaseName = "Incorrect database name";
    public const string IncorrectTableName = "Incorrect table name";
    public const string IncorrectConnectionString = "Incorrect connection string";
    public const string DatabaseTypeIsNotSupported = "Specified database type is not supported";
    public const string IncorrectDatabaseType = "Incorrect database type";
    public const string UnableToGetResultForOpenForeachOperation = "Unable to get result for open foreach operation";
    public const string UnableToAddActionForClosedForeachOperation = "Unable to add action for closed foreach operation";
    public const string ForeachRequiresNotEmptyTableNames = "Foreach operation requires not empty list of table names";
    public const string DatabaseShouldBeConnected = "Database should be connected";
    public const string UnableToCommitNotOpenTransaction = "Unable to commit transaction: either connection or transactin was not open";
    public const string UnableToRollbackNotOpenTransaction = "Unable to rollback transaction: either connection or transactin was not open";
    public const string OrderingFieldNameCouldNotBeNullOrEmpty = "Ordering field name could not be null or empty";
    public const string TableNameCouldNotBeNullOrEmpty = "Table name cannot be null or empty";
    public const string ErrorInQueryBuilder = "Error in query builder";
    public const string UnableToCreateQueryBuilderForDbType = "Unable to create query builder for the specified database type";
    public const string QueryBuilderIsBuilt = "Query builder is already built";
    public const string QueryBuilderRequiresColumn = "Query builder requires at least one column for creating table";
    public const string IncorrectQueryBuilderResult = "Incorrect query builder result";
    public const string EmptyColumnInfoList = "Column info list could not be empty";
    public const string EmptyForeignKeyInfoList = "Foreign key info list could not be empty";
    public const string NumericNativeColumnTypeConversion = "Incorrect conversion to numeric native column type";
    public const string IncorrectBaseForNumericNativeColumnType = "Incorrect base type for numeric native column type";
    public const string NumericScaleBiggerThanPrecision = "Numeric scale could not be bigger than precision";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

/// <summary>
/// Base class for testing get SQL definition operation.
/// </summary>
public abstract class BaseDbConnectionGetSqlFromDataTableTests
{
    private readonly DatabaseType _databaseType;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    protected BaseDbConnectionGetSqlFromDataTableTests(DatabaseType databaseType)
    {
        _databaseType = databaseType;
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("TestName")]
    public void GetSqlFromDataTable_DataTableIsNull_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);
        Action act = () => dbConnection.GetSqlFromDataTable(dt: null, tableName);

        // Act & Assert.
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetSqlFromDataTable_TableNameIsNullOrEmpty_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        DataTable dtEmployees = InitializeEmployeesDataTable();
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);
        Action act = () => dbConnection.GetSqlFromDataTable(dtEmployees, tableName);

        // Act & Assert.
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetSqlFromDataTable_EmptyDataTableAndTableNameIsNullOrEmpty_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);
        Action act = () => dbConnection.GetSqlFromDataTable(dt: new DataTable(), tableName: tableName);

        // Act & Assert.
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetSqlFromDataTable_EmptyDataTable_ThrowsArgumentException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);
        Action act = () => dbConnection.GetSqlFromDataTable(dt: new DataTable(), tableName: "TestDataTable");

        // Act & Assert.
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetSqlFromDataTable_EmployeesDataTableAndNotEmptyRows_SqlWithInsert()
    {
        // Arrange.
        DataTable dtEmployees = InitializeEmployeesDataTable();
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);
        string expectedSql = @"CREATE TABLE IF NOT EXISTS Employees (EmployeeID TEXT,FirstName TEXT,LastName TEXT,HireDate TEXT);
INSERT INTO Employees (EmployeeID,FirstName,LastName,HireDate) VALUES ('101','John','Doe','5/15/2020 12:00:00 AM');
INSERT INTO Employees (EmployeeID,FirstName,LastName,HireDate) VALUES ('102','Jane','Smith','8/20/2019 12:00:00 AM');
INSERT INTO Employees (EmployeeID,FirstName,LastName,HireDate) VALUES ('103','Peter','Jones','1/10/2021 12:00:00 AM');
";

        // Act.
        string resultSql = dbConnection.GetSqlFromDataTable(dt: dtEmployees, tableName: "Employees");

        // Assert.
        resultSql.Should().Be(expectedSql);
    }

    [Fact]
    public void GetSqlFromDataTable_EmployeesDataTableAndEmptyRows_SqlWithoutInsert()
    {
        // Arrange.
        DataTable dtEmployees = InitializeEmployeesDataTable(addRows: false);
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);
        string expectedSql = @"CREATE TABLE IF NOT EXISTS Employees (EmployeeID TEXT,FirstName TEXT,LastName TEXT,HireDate TEXT);
";

        // Act.
        string resultSql = dbConnection.GetSqlFromDataTable(dt: dtEmployees, tableName: "Employees");

        // Assert.
        resultSql.Should().Be(expectedSql);
    }

    private static DataTable InitializeEmployeesDataTable(bool addRows = true)
    {
        // 1. Create a DataTable instance
        DataTable dtEmployees = new("Employees");

        // 2. Define Columns
        dtEmployees.Columns.Add("EmployeeID", typeof(int));
        dtEmployees.Columns.Add("FirstName", typeof(string));
        dtEmployees.Columns.Add("LastName", typeof(string));
        dtEmployees.Columns.Add("HireDate", typeof(DateTime));

        // 3. Add Rows (Data)
        if (addRows)
        {
            dtEmployees.Rows.Add(101, "John", "Doe", new DateTime(2020, 5, 15));
            dtEmployees.Rows.Add(102, "Jane", "Smith", new DateTime(2019, 8, 20));
            dtEmployees.Rows.Add(103, "Peter", "Jones", new DateTime(2021, 1, 10));
        }

        return dtEmployees;
    }
}

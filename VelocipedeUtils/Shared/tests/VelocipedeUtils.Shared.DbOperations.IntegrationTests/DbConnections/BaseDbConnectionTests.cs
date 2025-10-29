using System.Data;
using System.Data.Common;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections
{
    public abstract class BaseDbConnectionTests
    {
        protected IDatabaseFixture _fixture;

        protected readonly string _connectionString;

        private readonly string _createTestModelsSql;
        private readonly string _createTestUsersSql;

        private const string SELECT_FROM_TESTMODELS = @"SELECT ""Id"", ""Name"" FROM ""TestModels""";
        private const string SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER = @"SELECT ""Id"", ""Name"" FROM ""TestModels"" WHERE ""Id"" >= @TestModelsId";

        protected BaseDbConnectionTests(IDatabaseFixture fixture, string sql, string createTestModelsSql, string createTestUsersSql)
        {
            _fixture = fixture;
            _connectionString = _fixture.ConnectionString;

            _createTestModelsSql = createTestModelsSql;
            _createTestUsersSql = createTestUsersSql;

            CreateTestDatabase(sql);
            InitializeTestDatabase();
        }

        [Fact]
        public async Task InitializeFixtureDatabase_CanRunQuery()
        {
            // Arrange.
            const int expected = 1;
            await using DbConnection connection = _fixture.GetDbConnection();
            connection.Open();

            // Act.
            int result = await connection.QueryFirstAsync<int>("SELECT 1");

            // Assert.
            result.Should().Be(expected);
        }

        [Fact]
        public void QueryFirstOrDefault_OpenDbAndGetOneRecord_ResultEqualsToExpected()
        {
            // Arrange.
            const int expected = 1;
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .QueryFirstOrDefault("SELECT 1", out int result);
            dbConnection.IsConnected.Should().BeTrue();
            dbConnection
                .CloseDb();

            // Assert.
            dbConnection.Should().NotBeNull();
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().Be(expected);
        }

        [Fact]
        public void Query_WithoutRestrictions_GetAllTestModels()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            List<TestModel> expected = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            };

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .Query(SELECT_FROM_TESTMODELS, out List<TestModel> result)
                .CloseDb();

            // Assert.
            result.Should().HaveCount(8);
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Query_WithParams_GetAllTestModelsWithIdBiggerThan5()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
            List<TestModel> expected = new List<TestModel>
            {
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            };

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .Query(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, out List<TestModel> result)
                .CloseDb();

            // Assert.
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Query_WithParamsAndDelegate_GetAllTestModelsWithIdBiggerThan5AndLessThan7()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
            Func<TestModel, bool> predicate = x => x.Id <= 7;
            List<TestModel> expected = new List<TestModel>
            {
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
            };

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .Query(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate, out List<TestModel> result)
                .CloseDb();

            // Assert.
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Query_WithDelegate_GetAllTestModelsWithIdLessThan7()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Func<TestModel, bool> predicate = x => x.Id <= 7;
            List<TestModel> expected = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
            };

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .Query(
                    SELECT_FROM_TESTMODELS,
                    parameters: null,
                    predicate: predicate,
                    result: out List<TestModel> result)
                .CloseDb();

            // Assert.
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public void QueryDataTable_FixtureWithoutRestrictions_GetAllTestModels()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            DataTable expected = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            }.Select(x => new { x.Id, x.Name }).ToDataTable();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .QueryDataTable(SELECT_FROM_TESTMODELS, out DataTable result)
                .CloseDb();

            // Assert.
            result.Rows.Count.Should().Be(8);
            AreDataTablesEquivalent(result, expected).Should().BeTrue();
        }

        [Fact]
        public void QueryDataTable_FixtureWithParams_GetTestModelsWithIdBiggerThan5()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
            DataTable expected = new List<TestModel>
            {
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            }.Select(x => new { x.Id, x.Name }).ToDataTable();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .QueryDataTable(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, out DataTable result)
                .CloseDb();

            // Assert.
            AreDataTablesEquivalent(result, expected).Should().BeTrue();
        }

        [Fact]
        public void QueryDataTable_FixtureWithParamsAndDelegate_GetTestModelsWithIdBiggerThan5AndLessThan7()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            List<VelocipedeCommandParameter>? parameters = [new() { Name = "TestModelsId", Value = 5 }];
            Func<dynamic, bool> predicate = x => x.Id <= 7;
            DataTable expected = new List<TestModel>
            {
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
            }.Select(x => new { x.Id, x.Name }).ToDataTable();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .QueryDataTable(SELECT_FROM_TESTMODELS_WHERE_ID_BIGGER, parameters, predicate, out DataTable result)
                .CloseDb();

            // Assert.
            AreDataTablesEquivalent(result, expected).Should().BeTrue();
        }

        [Fact]
        public void QueryDataTable_FixtureWithDelegate_GetTestModelsWithIdLessThan7()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Func<dynamic, bool> predicate = x => x.Id <= 7;
            DataTable expected = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
            }.Select(x => new { x.Id, x.Name }).ToDataTable();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .QueryDataTable(
                    SELECT_FROM_TESTMODELS,
                    parameters: null,
                    predicate: predicate,
                    dtResult: out DataTable result)
                .CloseDb();

            // Assert.
            AreDataTablesEquivalent(result, expected).Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void QueryDataTable_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.QueryDataTable(SELECT_FROM_TESTMODELS, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        }

        [Fact]
        public void QueryDataTable_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.QueryDataTable(SELECT_FROM_TESTMODELS, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void QueryDataTable_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.QueryDataTable(SELECT_FROM_TESTMODELS, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void GetAllDataFromTable_ConnectionStringFromFixtureAndGetAllTestModelsAsDataTable_QuantityEqualsToSpecified()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            DataTable expected = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            }.ToDataTable();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .GetAllDataFromTable("\"TestModels\"", out DataTable result)
                .CloseDb();

            // Assert.
            result.Rows.Count.Should().Be(8);
            AreDataTablesEquivalent(result, expected).Should().BeTrue();
        }

        [Fact]
        public void GetAllDataFromTable_ConnectionStringFromFixtureAndGetAllTestModelsAsList_QuantityEqualsToSpecified()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            List<TestModel> expected = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            };

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .GetAllDataFromTable("\"TestModels\"", out List<TestModel> result)
                .CloseDb();

            // Assert.
            result.Count.Should().Be(8);
            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAllDataFromTable_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetAllDataFromTable("\"TestModels\"", out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        }

        [Fact]
        public void GetAllDataFromTable_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetAllDataFromTable("\"TestModels\"", out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void GetAllDataFromTable_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetAllDataFromTable("\"TestModels\"", out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void DbExists_ConnectionStringFromFixture_ReturnsTrue()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            bool result = dbConnection.DbExists();

            // Assert.
            result.Should().BeTrue();
        }

        [Fact]
        public void DbExists_ConnectAndSetNotExistingDb_ReturnsFalse()
        {
            // Arrange.
            string dbName = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection
                .GetConnectionString(dbName, out string newConnectionString)
                .SetConnectionString(newConnectionString);

            // Act.
            bool result = dbConnection.DbExists();

            // Assert.
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DbExists_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.DbExists();

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        }

        [Fact]
        public void DbExists_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.DbExists();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void DbExists_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.DbExists();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void CreateDb_ConnectionStringFromFixture_ThrowsInvalidOperationException()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Action act = () => dbConnection.CreateDb();

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.DatabaseAlreadyExists);
        }

        [Fact]
        public virtual void CreateDb_ConnectAndSetNotExistingDbUsingSetters_DbExists()
        {
            // Arrange.
            string dbName = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            if (_fixture.DatabaseType == DatabaseType.SQLite)
                dbName = $"{dbName}.db";

            // Act.
            dbConnection
                .OpenDb()
                .GetConnectionString(dbName, out string newConnectionString)
                .SetConnectionString(newConnectionString)
                .CreateDb()
                .SwitchDb(dbName)
                .CloseDb();
            bool dbExists = dbConnection.DbExists();

            // Assert.
            dbExists.Should().BeTrue();
        }

        [Fact]
        public virtual void CreateDb_CreateNotExistingDbUsingExtensionMethod_DbExists()
        {
            // Arrange.
            string dbName = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection
                .OpenDb()
                .CreateDb(dbName)
                .SwitchDb(dbName)
                .CloseDb();
            bool dbExists = dbConnection.DbExists();

            // Assert.
            dbExists.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CreateDb();

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void CreateDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CreateDb();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void CreateDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CreateDb();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void CreateDbIfNotExists_ConnectionStringFromFixture_NotThrowAnyException()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Action act = () => dbConnection.CreateDbIfNotExists();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateDbIfNotExists_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CreateDbIfNotExists();

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void CreateDbIfNotExists_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CreateDbIfNotExists();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void OpenDb_ConnectionStringFromFixture_NotThrowAnyException()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Action act = () => dbConnection.OpenDb();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.IsConnected.Should().BeTrue();
        }

        [Fact]
        public void OpenDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.OpenDb();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void OpenDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.OpenDb();

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void OpenDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.OpenDb();

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void CloseDb_ConnectionStringFromFixture_NotThrowAnyException()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Action act = () => dbConnection.CloseDb();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void CloseDb_ConnectionStringFromFixtureAndConnected_NotThrowAnyException()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.OpenDb();
            Action act = () => dbConnection.CloseDb();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void CloseDb_GuidInsteadOfConnectionString_NotThrowAnyException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CloseDb();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CloseDb_NullOrEmptyConnectionString_NotThrowAnyException(string? connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CloseDb();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void CloseDb_IncorrectConnectionString_NotThrowAnyException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.CloseDb();

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void GetTablesInDb_ConnectionStringFromFixtureAndNotConnected_ResultContainsAllExpectedStrings()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            var expected = new List<string>
            {
                "TestModels",
                "TestUsers",
                "TestCities",
            };

            // Act.
            dbConnection.GetTablesInDb(out List<string> result);
            result = result
                .Select(x => x.Replace("public.", ""))
                .ToList();

            // Assert.
            result.Should().HaveCountGreaterThanOrEqualTo(3);
            foreach (var expectedString in expected)
            {
                result.Should().Contain(expectedString);
            }
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void GetTablesInDb_ConnectionStringFromFixtureAndConnected_ResultContainsAllExpectedStrings()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            var expected = new List<string>
            {
                "TestModels",
                "TestUsers",
                "TestCities",
            };

            // Act.
            dbConnection
                .OpenDb()
                .GetTablesInDb(out List<string> result)
                .CloseDb();
            result = result
                .Select(x => x.Replace("public.", ""))
                .ToList();

            // Assert.
            result.Should().HaveCountGreaterThanOrEqualTo(3);
            foreach (var expectedString in expected)
            {
                result.Should().Contain(expectedString);
            }
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void GetTablesInDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetTablesInDb(out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetTablesInDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetTablesInDb(out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void GetTablesInDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetTablesInDb(out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void GetColumns_ConnectionStringFromFixtureAndNotConnected_ResultContainsAllExpectedStrings()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            string tableName = "\"TestModels\"";

            // Act.
            dbConnection.GetColumns(tableName, out List<VelocipedeColumnInfo>? result);

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetColumns_ConnectionStringFromFixtureAndConnected_ResultContainsAllExpectedStrings()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            string tableName = "\"TestModels\"";

            // Act.
            dbConnection
                .OpenDb()
                .GetColumns(tableName, out List<VelocipedeColumnInfo>? result)
                .CloseDb();
            
            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetColumns_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetColumns(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetColumns_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetColumns(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void GetColumns_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetColumns(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("\"TestModels\"", 0)]
        [InlineData("\"TestUsers\"", 1)]
        public void GetForeignKeys_ConnectionStringFromFixtureAndNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection.GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? result);

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().HaveCount(expectedQty);
        }

        [Theory]
        [InlineData("\"TestModels\"", 0)]
        [InlineData("\"TestUsers\"", 1)]
        public void GetForeignKeys_ConnectionStringFromFixtureAndConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection
                .OpenDb()
                .GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? result)
                .CloseDb();

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().HaveCount(expectedQty);
        }

        [Fact]
        public void GetForeignKeys_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetForeignKeys(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetForeignKeys_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetForeignKeys(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void GetForeignKeys_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetForeignKeys(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("\"TestModels\"", 0)]
        [InlineData("\"TestUsers\"", 2)]
        public void GetTriggers_ConnectionStringFromFixtureAndNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection.GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result);

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().HaveCount(expectedQty);
        }

        [Theory]
        [InlineData("\"TestModels\"", 0)]
        [InlineData("\"TestUsers\"", 2)]
        public void GetTriggers_ConnectionStringFromFixtureAndConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection
                .OpenDb()
                .GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result)
                .CloseDb();

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().HaveCount(expectedQty);
        }

        [Fact]
        public void GetTriggers_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetTriggers(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetTriggers_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetTriggers(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void GetTriggers_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetTriggers(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("\"TestModels\"")]
        [InlineData("\"TestUsers\"")]
        public virtual void GetSqlDefinition_ConnectionStringFromFixtureAndNotConnected_ResultEqualsToExpected(string tableName)
        {
            // Arrange.
            string expected = GetExpectedSqlDefinition(tableName);
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection.GetSqlDefinition(tableName, out string? result);
            result = result?.Replace("\r\n", "\n");
            expected = expected.Replace("\r\n", "\n");

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("\"TestModels\"")]
        [InlineData("\"TestUsers\"")]
        public virtual void GetSqlDefinition_ConnectionStringFromFixtureAndConnected_ResultEqualsToExpected(string tableName)
        {
            // Arrange.
            string expected = GetExpectedSqlDefinition(tableName);
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection
                .OpenDb()
                .GetSqlDefinition(tableName, out string? result)
                .CloseDb();
            result = result?.Replace("\r\n", "\n");
            expected = expected.Replace("\r\n", "\n");

            // Assert.
            dbConnection.IsConnected.Should().BeFalse();
            result.Should().Be(expected);
        }

        [Fact]
        public void GetSqlDefinition_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            string connectionString = Guid.NewGuid().ToString();
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetSqlDefinition_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT CONNECTION STRING")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-connection-string")]
        public void GetSqlDefinition_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
        {
            // Arrange.
            string tableName = "\"TestModels\"";
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            dbConnection.SetConnectionString(connectionString);
            Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>()
                .WithInnerException(typeof(ArgumentException));
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void SwitchDb_DbNameFromFixture_ReconnectedWithSameConnectionString()
        {
            // Arrange.
            string connectionString = string.Empty;
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            string? dbName = dbConnection.GetActiveDatabaseName();
            Action act = () => dbConnection.SwitchDb(dbName, out connectionString);

            // Act & Assert.
            act.Should().NotThrow<Exception>();
            dbConnection.ConnectionString.Should().Be(connectionString);
            dbConnection.ConnectionString.Should().Be(_fixture.ConnectionString);
            dbConnection.IsConnected.Should().BeTrue();
        }

        [Fact]
        public void SwitchDb_GuidInsteadOfDbName_ThrowsVelocipedeDbConnectParamsException()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            string dbName = Guid.NewGuid().ToString();
            Action act = () => dbConnection.SwitchDb(dbName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SwitchDb_NullOrEmptyDbName_ThrowsInvalidOperationException(string? dbName)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Action act = () => dbConnection.SwitchDb(dbName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbNameException>()
                .WithMessage(ErrorMessageConstants.IncorrectDatabaseName);
            dbConnection.IsConnected.Should().BeFalse();
        }

        [Theory]
        [InlineData("INCORRECT DATABASE NAME")]
        [InlineData("-;")]
        [InlineData("`<3;")]
        [InlineData("connect:localhost:0000;")]
        [InlineData("connect:localhost:0000;super-database-name")]
        [InlineData("super-not-existing-database-name")]
        public void SwitchDb_IncorrectDbName_ThrowsVelocipedeDbConnectParamsException(string dbName)
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
            Action act = () => dbConnection.SwitchDb(dbName, out _);

            // Act & Assert.
            act
                .Should()
                .Throw<VelocipedeDbConnectParamsException>();
            dbConnection.IsConnected.Should().BeFalse();
        }

        private void CreateTestDatabase(string sql)
        {
            using DbConnection connection = _fixture.GetDbConnection();
            connection.Open();
            connection.Execute(sql);
        }

        private void InitializeTestDatabase()
        {
            using TestDbContext dbContext = _fixture.GetTestDbContext();

            // Test models.
            var testModels = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test_1" },
                new TestModel { Id = 2, Name = "Test_2" },
                new TestModel { Id = 3, Name = "Test_3" },
                new TestModel { Id = 4, Name = "Test_4" },
                new TestModel { Id = 5, Name = "Test_5" },
                new TestModel { Id = 6, Name = "Test_6" },
                new TestModel { Id = 7, Name = "Test_7" },
                new TestModel { Id = 8, Name = "Test_8" },
            };
            foreach (TestModel testModel in testModels)
            {
                int? existingId = dbContext.TestModels.FirstOrDefault(x => x.Id == testModel.Id)?.Id;
                if (!existingId.HasValue)
                {
                    dbContext.TestModels.Add(testModel);
                }
            }

            // Cities.
            var cities = new List<TestCity>
            {
                new TestCity { Id = 1, Name = "City_1" },
                new TestCity { Id = 2, Name = "City_2" },
                new TestCity { Id = 3, Name = "City_3" },
                new TestCity { Id = 4, Name = "City_4" },
            };
            foreach (TestCity city in cities)
            {
                int? existingId = dbContext.TestCities.FirstOrDefault(x => x.Id == city.Id)?.Id;
                if (!existingId.HasValue)
                {
                    dbContext.TestCities.Add(city);
                }
            }
            TestCity? firstCity = cities.FirstOrDefault();
            TestCity? lastCity = cities.LastOrDefault();

            // Users.
            var users = new List<TestUser>
            {
                new TestUser { Id = 1, Name = "User_1", Email = "User_1@example.com", CityId = lastCity?.Id, City = lastCity },
                new TestUser { Id = 2, Name = "User_2", Email = "User_2@example.com" },
                new TestUser { Id = 3, Name = "User_3", Email = "User_3@example.com" },
                new TestUser { Id = 4, Name = "User_4", Email = "User_4@example.com", CityId = firstCity?.Id, City = firstCity },
                new TestUser { Id = 5, Name = "User_5", Email = "User_5@example.com", CityId = firstCity?.Id, City = firstCity },
                new TestUser { Id = 6, Name = "User_6", Email = "User_6@example.com" },
                new TestUser { Id = 7, Name = "User_7", Email = "User_7@example.com", CityId = firstCity?.Id, City = firstCity },
                new TestUser { Id = 8, Name = "User_8", Email = "User_8@example.com" },
                new TestUser { Id = 9, Name = "User_9", Email = "User_9@example.com", CityId = lastCity?.Id, City = lastCity },
                new TestUser { Id = 10, Name = "User_10", Email = "User_10@example.com", CityId = lastCity?.Id, City = lastCity },
                new TestUser { Id = 11, Name = "User_11", Email = "User_11@example.com" },
            };
            foreach (TestUser user in users)
            {
                int? existingId = dbContext.TestUsers.FirstOrDefault(x => x.Id == user.Id)?.Id;
                if (!existingId.HasValue)
                {
                    dbContext.TestUsers.Add(user);
                }
            }

            dbContext.SaveChanges();
        }

        private string GetExpectedSqlDefinition(string tableName)
        {
            tableName = tableName.Trim('"');
            return tableName switch
            {
                "TestModels" => _createTestModelsSql,
                "TestUsers" => _createTestUsersSql,
                _ => "",
            };
        }

        private static bool CompareDataTableSchema(DataTable dt1, DataTable dt2)
        {
            if (dt1.Columns.Count != dt2.Columns.Count)
                return false;

            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                if (dt1.Columns[i].ColumnName != dt2.Columns[i].ColumnName ||
                    dt1.Columns[i].DataType != dt2.Columns[i].DataType)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CompareDataTableContent(DataTable dt1, DataTable dt2)
        {
            if (dt1.Rows.Count != dt2.Rows.Count)
                return false;

            // Ensure consistent order for comparison if not already sorted
            // You might need to sort both DataTables by a common key before this step
            // For example: dt1.DefaultView.Sort = "ColumnName ASC"; dt1 = dt1.DefaultView.ToTable();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (!dt1.Rows[i].ItemArray.SequenceEqual(dt2.Rows[i].ItemArray))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool AreDataTablesEquivalent(DataTable dt1, DataTable dt2)
        {
            if (dt1 == null || dt2 == null)
                return dt1 == dt2; // Both null is equivalent, one null is not

            if (!CompareDataTableSchema(dt1, dt2))
                return false;

            if (!CompareDataTableContent(dt1, dt2))
                return false;

            return true;
        }
    }
}

using System.Data;
using System.Data.Common;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
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

        protected BaseDbConnectionTests(IDatabaseFixture fixture, string sql)
        {
            _fixture = fixture;
            _connectionString = _fixture.ConnectionString;

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
        public void Query_GetAllTestModels_QuantityEqualsToSpecified()
        {
            // Arrange.
            using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

            // Act.
            dbConnection.IsConnected.Should().BeFalse();
            dbConnection
                .OpenDb()
                .Query(@"SELECT ""Id"", ""Name"" from ""TestModels""", out List<TestModel> result)
                .CloseDb();

            // Assert.
            result.Should().HaveCount(8);
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
    }
}

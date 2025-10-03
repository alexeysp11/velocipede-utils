using System.Data.Common;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base
{
    public abstract class BaseDbConnectionQueryTests
    {
        protected IDatabaseFixture _fixture;
        protected readonly string _connectionString;

        private TestDbContext _dbContext;

        protected BaseDbConnectionQueryTests(IDatabaseFixture fixture)
        {
            _fixture = fixture;
            _connectionString = _fixture.ConnectionString;
            _dbContext = InitializeTestDatabase();
        }

        [Fact]
        public async Task InitializeDatabaseInContainers_CanRunQuery()
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
            IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

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

        private TestDbContext InitializeTestDatabase()
        {
            TestDbContext dbContext = _fixture.GetTestDbContext();

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
            dbContext.TestModels.AddRange(testModels);

            // Cities.
            var cities = new List<TestCity>
            {
                new TestCity { Id = 1, Name = "City_1" },
                new TestCity { Id = 2, Name = "City_2" },
                new TestCity { Id = 3, Name = "City_3" },
                new TestCity { Id = 4, Name = "City_4" },
            };
            TestCity? firstCity = cities.FirstOrDefault();
            TestCity? lastCity = cities.LastOrDefault();
            dbContext.TestCities.AddRange(cities);

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
            dbContext.TestUsers.AddRange(users);

            dbContext.Database.Migrate();
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}

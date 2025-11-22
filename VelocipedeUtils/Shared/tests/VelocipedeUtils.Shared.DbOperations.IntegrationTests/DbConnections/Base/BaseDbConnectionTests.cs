using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection"/>.
/// </summary>
public abstract class BaseDbConnectionTests
{
    protected IDatabaseFixture _fixture;

    protected readonly string _connectionString;

    protected const string SelectFromTestModels = @"SELECT ""Id"", ""Name"" FROM ""TestModels""";
    protected const string SelectFromTestModelsWhereIdBigger = @"SELECT ""Id"", ""Name"" FROM ""TestModels"" WHERE ""Id"" >= @TestModelsId";

    /// <summary>
    /// Default constructor for creating <see cref="BaseDbConnectionTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseDbConnectionTests(IDatabaseFixture fixture, string createDatabaseSql)
    {
        _fixture = fixture;
        _connectionString = _fixture.ConnectionString;

        CreateTestDatabase(createDatabaseSql);
        InitializeTestDatabase();
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
            new() { Id = 1, Name = "Test_1" },
            new() { Id = 2, Name = "Test_2" },
            new() { Id = 3, Name = "Test_3" },
            new() { Id = 4, Name = "Test_4" },
            new() { Id = 5, Name = "Test_5" },
            new() { Id = 6, Name = "Test_6" },
            new() { Id = 7, Name = "Test_7" },
            new() { Id = 8, Name = "Test_8" },
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
            new() { Id = 1, Name = "City_1" },
            new() { Id = 2, Name = "City_2" },
            new() { Id = 3, Name = "City_3" },
            new() { Id = 4, Name = "City_4" },
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
            new() { Id = 1, Name = "User_1", Email = "User_1@example.com", CityId = lastCity?.Id, City = lastCity },
            new() { Id = 2, Name = "User_2", Email = "User_2@example.com" },
            new() { Id = 3, Name = "User_3", Email = "User_3@example.com" },
            new() { Id = 4, Name = "User_4", Email = "User_4@example.com", CityId = firstCity?.Id, City = firstCity },
            new() { Id = 5, Name = "User_5", Email = "User_5@example.com", CityId = firstCity?.Id, City = firstCity },
            new() { Id = 6, Name = "User_6", Email = "User_6@example.com" },
            new() { Id = 7, Name = "User_7", Email = "User_7@example.com", CityId = firstCity?.Id, City = firstCity },
            new() { Id = 8, Name = "User_8", Email = "User_8@example.com" },
            new() { Id = 9, Name = "User_9", Email = "User_9@example.com", CityId = lastCity?.Id, City = lastCity },
            new() { Id = 10, Name = "User_10", Email = "User_10@example.com", CityId = lastCity?.Id, City = lastCity },
            new() { Id = 11, Name = "User_11", Email = "User_11@example.com" },
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

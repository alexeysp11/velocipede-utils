using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

/// <summary>
/// Database context that contains test models.
/// </summary>
public class TestDbContext : DbContext
{
#nullable disable
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
#nullable restore

    /// <summary>
    /// Collection of test users.
    /// </summary>
    public DbSet<TestUser> TestUsers { get; set; }

    /// <summary>
    /// Collection of test cities.
    /// </summary>
    public DbSet<TestCity> TestCities { get; set; }

    /// <summary>
    /// Collection of test models.
    /// </summary>
    public DbSet<TestModel> TestModels { get; set; }
}

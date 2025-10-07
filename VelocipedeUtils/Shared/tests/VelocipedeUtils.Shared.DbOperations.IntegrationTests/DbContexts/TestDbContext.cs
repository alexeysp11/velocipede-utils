using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts
{
    public class TestDbContext : DbContext
    {
#nullable disable
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
#nullable restore

        public DbSet<TestUser> TestUsers { get; set; }
        public DbSet<TestCity> TestCities { get; set; }
        public DbSet<TestModel> TestModels { get; set; }
    }
}

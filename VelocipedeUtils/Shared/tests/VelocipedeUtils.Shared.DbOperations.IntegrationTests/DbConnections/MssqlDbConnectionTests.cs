using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections
{
    public sealed class MssqlDbConnectionTests : BaseDbConnectionTests, IClassFixture<MssqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
IF OBJECT_ID(N'dbo.""TestModels""', N'U') IS NULL
BEGIN
    create table dbo.""TestModels"" (
        ""Id"" int primary key,
        ""Name"" varchar(50) NOT NULL,
        ""AdditionalInfo"" varchar(50) NULL);
END;

IF OBJECT_ID(N'dbo.""TestCities""', N'U') IS NULL
BEGIN
    create table dbo.""TestCities"" (
        ""Id"" int primary key,
        ""Name"" varchar(50) NOT NULL);
END;

IF OBJECT_ID(N'dbo.""TestUsers""', N'U') IS NULL
BEGIN
    create table dbo.""TestUsers"" (
        ""Id"" int primary key,
        ""Name"" varchar(50) NOT NULL,
        ""Email"" varchar(50) NOT NULL,
        ""CityId"" int NULL,
        ""AdditionalInfo"" varchar(50) NULL,
        FOREIGN KEY (""CityId"") REFERENCES ""TestCities""(""Id""));
END;";

        public MssqlDbConnectionTests(MssqlDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}

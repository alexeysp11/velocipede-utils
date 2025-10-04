// This class is corrently excluded due to .NET MySQL errors "The given key was not present in the dictionary".

#define EXCLUDE_MYSQL_PROVIDER

#if !EXCLUDE_MYSQL_PROVIDER

using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MysqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
SET sql_mode='ANSI_QUOTES';

create table if not exists ""TestModels"" (
    ""Id"" int primary key,
    ""Name"" varchar(50) NOT NULL,
    ""AdditionalInfo"" varchar(50));

create table if not exists ""TestCities"" (
    ""Id"" int primary key,
    ""Name"" varchar(50) NOT NULL);

create table if not exists ""TestUsers"" (
    ""Id"" int primary key,
    ""Name"" varchar(50) NOT NULL,
    ""Email"" varchar(50) NOT NULL,
    ""CityId"" int,
    ""AdditionalInfo"" varchar(50) NULL,
    FOREIGN KEY (""CityId"") REFERENCES ""TestCities""(""Id""));";

        public MysqlDbConnectionQueryTests(MysqlDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}

#endif

using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MysqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
create table if not exists TestModels (
    Id int primary key,
    Name varchar(50) NOT NULL,
    AdditionalInfo varchar(50));

create table if not exists TestCities (
    Id int primary key,
    Name varchar(50) NOT NULL);

create table if not exists TestUsers (
    Id int primary key,
    Name varchar(50) NOT NULL,
    Email varchar(50) NOT NULL,
    CityId int,
    AdditionalInfo varchar(50) NULL,
    FOREIGN KEY (CityId) REFERENCES TestCities(Id));";

        public MysqlDbConnectionQueryTests(MysqlDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}

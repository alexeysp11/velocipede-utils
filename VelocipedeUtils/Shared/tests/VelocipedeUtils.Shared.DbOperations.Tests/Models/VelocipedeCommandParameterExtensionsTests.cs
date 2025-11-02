using System.Data;
using Dapper;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.Tests.Models
{
    public sealed class VelocipedeCommandParameterExtensionsTests
    {
        [Theory]
        [InlineData("Name_1", 1, DbType.Int32, ParameterDirection.Input)]
        [InlineData("Name_1", null, DbType.Int32, ParameterDirection.Input)]
        [InlineData("Name_2", 21, DbType.Int32, ParameterDirection.InputOutput)]
        [InlineData("Name_3", -2, DbType.Int32, ParameterDirection.Output)]
        [InlineData("Name_3", null, DbType.Int32, ParameterDirection.Output)]
        public void ToDapperParameters_PassInt32_GetSpecifiedValue(string name, int? value, DbType dbType, ParameterDirection direction)
        {
            // Arrange.
            List<VelocipedeCommandParameter> parameters = [new() { Name = name, Value = value, DbType = dbType, Direction = direction }];

            // Act.
            DynamicParameters? dynamicParameters = parameters.ToDapperParameters();
            int? parameter = dynamicParameters?.Get<int?>(name);

            // Assert.
            parameter.Should().Be(value);
        }

        [Theory]
        [InlineData(DbType.DateTime, ParameterDirection.Input)]
        [InlineData(DbType.DateTime, ParameterDirection.InputOutput)]
        [InlineData(DbType.DateTime, ParameterDirection.Output)]
        [InlineData(DbType.Date, ParameterDirection.Input)]
        [InlineData(DbType.Date, ParameterDirection.InputOutput)]
        [InlineData(DbType.Date, ParameterDirection.Output)]
        [InlineData(DbType.DateTime2, ParameterDirection.Input)]
        [InlineData(DbType.DateTime2, ParameterDirection.InputOutput)]
        [InlineData(DbType.DateTime2, ParameterDirection.Output)]
        [InlineData(DbType.DateTimeOffset, ParameterDirection.Input)]
        [InlineData(DbType.DateTimeOffset, ParameterDirection.InputOutput)]
        [InlineData(DbType.DateTimeOffset, ParameterDirection.Output)]
        public void ToDapperParameters_Date(DbType dbType, ParameterDirection direction)
        {
            // Arrange.
            string name = "Name_1";
            DateTime value = DateTime.UtcNow;
            List<VelocipedeCommandParameter>? parameters = [new() { Name = name, Value = value, DbType = dbType, Direction = direction }];

            // Act.
            DynamicParameters? dynamicParameters = parameters?.ToDapperParameters();
            DateTime? parameter = dynamicParameters?.Get<DateTime?>(name);

            // Assert.
            parameter.Should().Be(value);
        }

        [Fact]
        public void ToDapperParameters_NullObject()
        {
            // Arrange.
            List<VelocipedeCommandParameter>? parameters = null;

            // Act.
#pragma warning disable CA1508 // Avoid dead conditional code
            DynamicParameters? dynamicParameters = parameters?.ToDapperParameters();
#pragma warning restore CA1508 // Avoid dead conditional code

            // Assert.
            dynamicParameters.Should().BeNull();
        }
    }
}

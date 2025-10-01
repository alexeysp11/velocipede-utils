using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class OracleDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public OracleDbConnectionInitializeTests() : base(DatabaseType.Oracle)
        {
            _connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=myusername;Password=mypassword;";
        }

        [Theory]
        [InlineData("Data Source=ORCL;", "ORCL")]
        [InlineData("Data Source=ORCL;User ID=scott;", "ORCL")]
        [InlineData("Data Source=ORCL;User ID=scott;Password=tiger;", "ORCL")]
        [InlineData("Data Source=ORCL;User ID=scott;Password=tiger;Connection Lifetime=60;", "ORCL")]
        [InlineData("Data Source=ORCL;User ID=scott;Password=tiger;Pooling=true;Min Pool Size=5;Connection Lifetime=60;", "ORCL")]
        [InlineData("Data Source=ORCL;User ID=scott;Password=tiger;Pooling=true;Min Pool Size=5;Max Pool Size=20;Connection Lifetime=60;", "ORCL")]
        [InlineData("Data Source=ORCL;User ID=scott;Password=tiger;Connection Lifetime=60;Enlist=true;", "ORCL")]
        [InlineData("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyService)));", "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyService)))")]
        [InlineData("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=scott;", "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=MyPort))(CONNECT_DATA=(SERVICE_NAME=MyService)))")]
        public void GetDatabaseName_StaticGetter_DatabaseNameEqualsToExpected(string connectionString, string expectedPath)
        {
            // Arrange & Act.
            string resultPath = OracleDbConnection.GetDatabaseName(connectionString);

            // Assert.
            resultPath.Should().Be(expectedPath);
        }
    }
}

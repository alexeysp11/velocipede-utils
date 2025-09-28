using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public class MssqlDbConnectionTest 
    {
        private string ConnectionString = "Data Source=LAPTOP\\SQLEXPRESS;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Fact(Skip = "This test is currently failing due to dependencies on the real database")]
        public void ExecuteSqlCommand_CorrectConnectionString_DataRetrieved()
        {
            // Arrange.
            string employeeName = "test_employee";
            string sqlSelect = $"SELECT * FROM studying.dbo.emp WHERE name = '{employeeName}'";
            string sqlInsert = $"INSERT INTO studying.dbo.emp (emp_id, name) SELECT MAX(e.emp_id), '{employeeName}' FROM studying.dbo.emp e WHERE NOT EXISTS ({sqlSelect})";
            string sqlDelete = $"DELETE FROM studying.dbo.emp WHERE name = '{employeeName}'";

            ICommonDbConnection dbConnection = new MssqlDbConnection(ConnectionString);

            // Act.
            dbConnection
                .ExecuteSqlCommand(sqlInsert)
                .ExecuteSqlCommand(sqlSelect, out DataTable dtSelectResult)
                .ExecuteSqlCommand(sqlDelete);

            // Assert.
            Assert.True(dtSelectResult.Rows[dtSelectResult.Rows.Count-1]["name"].ToString() == employeeName);
        }
    }
}
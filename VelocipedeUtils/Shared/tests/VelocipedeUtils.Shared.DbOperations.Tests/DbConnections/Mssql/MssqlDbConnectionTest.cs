using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public class MssqlDbConnectionTest 
    {
        private readonly string ConnectionString = "Data Source=LAPTOP\\SQLEXPRESS;Trusted_Connection=True;MultipleActiveResultSets=true";

        [Fact(Skip = "This test is currently failing due to dependencies on the real database")]
        public void QueryDataTable_CorrectConnectionString_DataRetrieved()
        {
            // Arrange.
            string employeeName = "test_employee";
            string sqlSelect = $"SELECT * FROM studying.dbo.emp WHERE name = '{employeeName}'";
            string sqlInsert = $"INSERT INTO studying.dbo.emp (emp_id, name) SELECT MAX(e.emp_id), '{employeeName}' FROM studying.dbo.emp e WHERE NOT EXISTS ({sqlSelect})";
            string sqlDelete = $"DELETE FROM studying.dbo.emp WHERE name = '{employeeName}'";

            using MssqlDbConnection dbConnection = new(ConnectionString);

            // Act.
            dbConnection
                .Execute(sqlInsert)
                .QueryDataTable(sqlSelect, out DataTable dtSelectResult)
                .Execute(sqlDelete);

            // Assert.
            Assert.True(dtSelectResult.Rows[dtSelectResult.Rows.Count-1]["name"].ToString() == employeeName);
        }
    }
}
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// MS SQL database connection.
    /// </summary>
    public sealed class MssqlDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MSSQL;
        public string DatabaseName { get; }
        public bool IsConnected { get; private set; }

        public MssqlDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException($"Connection string should not be null or empty: {ConnectionString}");

            throw new System.NotImplementedException();
        }

        public void CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException($"Database already exists");

            throw new System.NotImplementedException();
        }

        public void OpenDb()
        {
            throw new System.NotImplementedException();
        }

        public void CloseDb()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayTablesInDb()
        {
            throw new System.NotImplementedException();
        }

        public void GetAllDataFromTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetColumnsOfTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetForeignKeys(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetTriggers(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void GetSqlDefinition(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public void ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlRequest, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            table = GetDataTable(reader);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return table;
        }

        private DataTable GetDataTable(SqlDataReader reader)
        {
            DataTable table = new DataTable();
            if (reader.FieldCount == 0) return table;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                DataColumn column;
                column = new DataColumn();
                column.ColumnName = reader.GetName(i);
                column.ReadOnly = true;
                table.Columns.Add(column);
            }
            while (reader.Read())
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader.GetValue(i).ToString();
                table.Rows.Add(row);
            }
            return table;
        }
    }
}

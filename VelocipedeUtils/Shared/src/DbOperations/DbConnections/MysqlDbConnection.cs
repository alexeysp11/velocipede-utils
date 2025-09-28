using System;
using System.Data;
using MySql.Data.MySqlClient;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// MySQL database connection.
    /// </summary>
    public sealed class MysqlDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MySQL;
        public string DatabaseName { get; }
        public bool IsConnected { get; private set; }

        public MysqlDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException($"Connection string should not be null or empty: {ConnectionString}");

            throw new System.NotImplementedException();
        }

        public ICommonDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException($"Database already exists");

            throw new System.NotImplementedException();
        }

        public ICommonDbConnection OpenDb()
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection CloseDb()
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetTablesInDb()
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetAllDataFromTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetColumnsOfTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetForeignKeys(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetTriggers(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection GetSqlDefinition(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public ICommonDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult)
        {
            dtResult = new DataTable();
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                var reader = new MySqlCommand(sqlRequest, connection).ExecuteReader();
                dtResult = GetDataTable(reader);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return this;
        }

        private DataTable GetDataTable(MySqlDataReader reader)
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

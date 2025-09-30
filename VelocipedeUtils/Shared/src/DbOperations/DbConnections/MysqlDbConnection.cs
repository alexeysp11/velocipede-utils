using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
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

        private MySqlConnection _connection;

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
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
            return this;
        }

        public ICommonDbConnection GetTablesInDb(out List<string> tables)
        {
            throw new NotImplementedException();

            string databaseName = "";
            string sql = $"SELECT table_name AS name FROM information_schema.tables WHERE table_schema = '{databaseName}';";
            Query(sql, out tables);
            return this;
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

        public ICommonDbConnection Query<T>(string sqlRequest, out List<T> result)
        {
            // Initialize connection.
            bool newConnectionUsed = true;
            MySqlConnection localConnection = null;
            if (_connection != null)
            {
                newConnectionUsed = false;
                localConnection = _connection;
            }
            else
            {
                localConnection = new MySqlConnection(ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                localConnection.Open();
            }

            // Execute SQL command and dispose connection if necessary.
            try
            {
                result = localConnection.Query<T>(sqlRequest).ToList();
            }
            finally
            {
                if (newConnectionUsed)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                    localConnection = null;
                }
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

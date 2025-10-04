// This class is corrently excluded due to .NET MySQL errors "The given key was not present in the dictionary".

#define EXCLUDE_MYSQL_PROVIDER

#if !EXCLUDE_MYSQL_PROVIDER

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
    public sealed class MysqlDbConnection : IVelocipedeDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MySQL;
        public string DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        private MySqlConnection _connection;

        public MysqlDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException($"Connection string should not be null or empty");

            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException($"Database already exists");

            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection OpenDb()
        {
            CloseDb();
            _connection = new MySqlConnection(ConnectionString);
            _connection.Open();
            return this;
        }

        public IVelocipedeDbConnection CloseDb()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
            return this;
        }

        public IVelocipedeDbConnection GetTablesInDb(out List<string> tables)
        {
            string sql = $"SELECT table_name AS name FROM information_schema.tables WHERE table_schema = '{DatabaseName}';";
            Query(sql, out tables);
            return this;
        }

        public IVelocipedeDbConnection GetColumns(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"
SELECT 
`COLUMN_NAME`,
`ORDINAL_POSITION`,
`COLUMN_DEFAULT`,
`IS_NULLABLE`,
`DATA_TYPE`,
`CHARACTER_MAXIMUM_LENGTH`,
`NUMERIC_PRECISION`,
`COLUMN_TYPE`,
`COLUMN_COMMENT`,
`GENERATION_EXPRESSION`
FROM `INFORMATION_SCHEMA`.`COLUMNS`
WHERE UPPER(`TABLE_SCHEMA`) LIKE UPPER('{0}')
AND UPPER(`TABLE_NAME`) LIKE UPPER('{1}')", DatabaseName, tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"
SELECT
    TABLE_NAME,COLUMN_NAME,
    CONSTRAINT_NAME,
    REFERENCED_TABLE_NAME,
    REFERENCED_COLUMN_NAME
FROM
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE
    UPPER(REFERENCED_TABLE_SCHEMA) LIKE UPPER('{0}')
    AND UPPER(REFERENCED_TABLE_NAME) LIKE UPPER('{1}');", DatabaseName, tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetTriggers(string tableName, out DataTable dtResult)
        {
            string sql = string.Format("SHOW TRIGGERS LIKE '{0}'", tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string sqlDefinition)
        {
            string sql = string.Format("SHOW CREATE TABLE {0}", tableName);
            return QueryFirstOrDefault(sql, out sqlDefinition);
        }

        public IVelocipedeDbConnection CreateTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection ClearTemporaryTable(string tableName)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection ExecuteSqlCommand(string sqlRequest, out DataTable dtResult)
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

        public IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result)
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

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T result)
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
                result = localConnection.QueryFirstOrDefault<T>(sqlRequest);
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

        /// <summary>
        /// Get database name by connection string.
        /// </summary>
        public static string GetDatabaseName(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = connectionString;
            return connectionStringBuilder.Database;
        }

        /// <summary>
        /// Get connection string by database name.
        /// </summary>
        public static string GetConnectionString(string databaseName)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder();
            connectionStringBuilder.Database = databaseName;
            return connectionStringBuilder.ConnectionString;
        }
    }
}

#endif

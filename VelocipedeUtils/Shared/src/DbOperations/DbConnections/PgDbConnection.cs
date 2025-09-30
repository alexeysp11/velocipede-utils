using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// PostgreSQL database connection.
    /// </summary>
    public sealed class PgDbConnection : ICommonDbConnection
    {
        private NpgsqlConnection _connection;

        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.PostgreSQL;
        public string DatabaseName { get; }
        public bool IsConnected { get; private set; }

        public PgDbConnection(string connectionString = null)
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
            string sql = "SELECT t.schemaname || '.' || t.relname AS name FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t";
            Query(sql, out tables);
            return this;
        }

        public ICommonDbConnection GetColumns(string tableName, out DataTable dtResult)
        {
            string[] tn = tableName.Split('.');
            string sql = string.Format(@"
SELECT
column_name,
ordinal_position,
column_default,
is_nullable,
data_type,
is_self_referencing,
is_generated,
is_updatable
FROM information_schema.columns
WHERE table_schema LIKE '{0}' AND table_name LIKE '{1}'", tn[0], tn[1]);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public ICommonDbConnection GetForeignKeys(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"
SELECT
    tc.table_schema,
    tc.constraint_name,
    tc.table_name,
    kcu.column_name,
    ccu.table_schema AS foreign_table_schema,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name
FROM 
    information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name AND ccu.table_schema = tc.table_schema
WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_name LIKE '{0}';", tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public ICommonDbConnection GetTriggers(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"SELECT * FROM information_schema.triggers WHERE event_object_table LIKE '{0}'", tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
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
            // Initialize connection.
            bool newConnectionUsed = true;
            NpgsqlConnection localConnection = null;
            if (_connection != null)
            {
                newConnectionUsed = false;
                localConnection = _connection;
            }
            else
            {
                localConnection = new NpgsqlConnection(ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                localConnection.Open();
            }

            // Execute SQL command and dispose connection if necessary.
            dtResult = new DataTable();
            try
            {
                using (var command = new NpgsqlCommand(sqlRequest, localConnection))
                {
                    var reader = command.ExecuteReader();
                    dtResult = GetDataTable(reader);
                    reader.Close();
                }
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

        public ICommonDbConnection Query<T>(string sqlRequest, out List<T> result)
        {
            // Initialize connection.
            bool newConnectionUsed = true;
            NpgsqlConnection localConnection = null;
            if (_connection != null)
            {
                newConnectionUsed = false;
                localConnection = _connection;
            }
            else
            {
                localConnection = new NpgsqlConnection(ConnectionString);
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

        private DataTable GetDataTable(NpgsqlDataReader reader)
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

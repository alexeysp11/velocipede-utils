using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Oracle database connection.
    /// </summary>
    public sealed class OracleDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.Oracle;
        public string DatabaseName { get; }
        public bool IsConnected { get; private set; }

        private OracleConnection _connection;

        public OracleDbConnection(string connectionString = null)
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
            string sql = "SELECT table_name AS name FROM user_tables";
            Query(sql, out tables);
            return this;
        }

        public ICommonDbConnection GetColumns(string tableName, out DataTable dtResult)
        {
            string[] tn = tableName.Split('.');
            string sql = string.Format(@"
SELECT column_name, data_type, data_length
FROM USER_TAB_COLUMNS
WHERE UPPER(table_name) = UPPER('{0}')", tn[0], tn[1]);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
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
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sqlRequest, con))
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        dtResult.Load(dr);
                    }
                }
            }
            return this;
        }

        public ICommonDbConnection Query<T>(string sqlRequest, out List<T> result)
        {
            // Initialize connection.
            bool newConnectionUsed = true;
            OracleConnection localConnection = null;
            if (_connection != null)
            {
                newConnectionUsed = false;
                localConnection = _connection;
            }
            else
            {
                localConnection = new OracleConnection(ConnectionString);
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
    }
}

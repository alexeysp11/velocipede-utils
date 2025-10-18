// The Oracle provider is not implemented yet.

#define EXCLUDE_ORACLE_PROVIDER

#if !EXCLUDE_ORACLE_PROVIDER

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
    public sealed class OracleDbConnection : IVelocipedeDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.Oracle;
        public string DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        private OracleConnection _connection;

        public OracleDbConnection(string? connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException(ErrorMessageConstants.DatabaseAlreadyExists);

            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection OpenDb()
        {
            CloseDb();
            _connection = new OracleConnection(ConnectionString);
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
            string sql = "SELECT table_name AS name FROM user_tables";
            Query(sql, out tables);
            return this;
        }

        public IVelocipedeDbConnection GetColumns(string tableName, out List<ColumnInfo> columnInfo)
        {
            string[] tn = tableName.Split('.');
            string sql = string.Format(@"
SELECT column_name, data_type, data_length
FROM USER_TAB_COLUMNS
WHERE UPPER(table_name) = UPPER('{0}')", tn[0], tn[1]);
            Query(sql, out columnInfo);
            return this;
        }

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"
SELECT *
FROM all_constraints
WHERE r_constraint_name IN (SELECT constraint_name FROM all_constraints WHERE UPPER(table_name) LIKE UPPER('{0}'))", tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetTriggers(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"SELECT * FROM all_triggers WHERE UPPER(table_name) LIKE UPPER('{0}')", tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition)
        {
            string sql = string.Format(@"
select dbms_metadata.get_ddl('TABLE', table_name) as sql
from user_tables ut
WHERE UPPER(ut.table_name) LIKE UPPER('{0}')", tableName);
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

        public IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result)
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

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T? result)
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

        /// <summary>
        /// Get database name by connection string.
        /// </summary>
        public static string GetDatabaseName(string connectionString)
        {
            var connectionStringBuilder = new OracleConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = connectionString;
            return connectionStringBuilder.DataSource;
        }

        /// <summary>
        /// Get connection string by database name.
        /// </summary>
        public static string GetConnectionString(string databaseName)
        {
            var connectionStringBuilder = new OracleConnectionStringBuilder();
            connectionStringBuilder.DataSource = databaseName;
            return connectionStringBuilder.ConnectionString;
        }
    }
}

#endif

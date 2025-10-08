using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// MS SQL database connection.
    /// </summary>
    public sealed class MssqlDbConnection : IVelocipedeDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MSSQL;
        public string DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        private SqlConnection _connection;

        public MssqlDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            if (_connection != null && _connection.ConnectionString == ConnectionString)
                return true;

            string existingConnectionString = _connection?.ConnectionString;
            try
            {
                OpenDb();
                CloseDb();
            }
            catch (VelocipedeDbConnectParamsException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                TryReconnect(existingConnectionString);
            }
            return true;
        }

        public IVelocipedeDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException(ErrorMessageConstants.DatabaseAlreadyExists);

            try
            {
                string sql = $"CREATE DATABASE \"{DatabaseName}\"";
                return Execute(sql);
            }
            catch (VelocipedeDbConnectParamsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbCreateException(ex);
            }
        }

        public IVelocipedeDbConnection OpenDb()
        {
            OpenDb(ConnectionString);
            return this;
        }

        /// <summary>
        /// Open database with the specified connection string.
        /// </summary>
        public IVelocipedeDbConnection OpenDb(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            try
            {
                CloseDb();
                connectionString = UsePersistSecurityInfo(connectionString);
                _connection = new SqlConnection(connectionString);
                _connection.Open();
                return this;
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Try to reconnect to the previous established connection.
        /// </summary>
        private bool TryReconnect(string connectionString)
        {
            try
            {
                OpenDb(connectionString);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Switch to the specified database and get new connection string.
        /// </summary>
        public IVelocipedeDbConnection SwitchDb(string dbName, out string connectionString)
        {
            if (string.IsNullOrEmpty(dbName))
                throw new VelocipedeDbNameException();

            try
            {
                // Change connection string.
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = ConnectionString;
                connectionStringBuilder.InitialCatalog = dbName;
                connectionString = connectionStringBuilder.ConnectionString;
                ConnectionString = connectionString;

                // Connect to the new database.
                OpenDb();

                return this;
            }
            catch (VelocipedeDbConnectParamsException)
            {
                CloseDb();
                throw;
            }
            catch (SqlException ex)
            {
                CloseDb();
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (ArgumentException ex)
            {
                CloseDb();
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                CloseDb();
                throw;
            }
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
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            Query(sql, out tables);
            return this;
        }

        public IVelocipedeDbConnection GetColumns(string tableName, out List<VelocipedeColumnInfo> columnInfo)
        {
            tableName = tableName.Trim('"');
            string sql = $@"
SELECT
    COLUMN_NAME as ColumnName,
    DATA_TYPE as ColumnType,
    COLUMN_DEFAULT as DefaultValue,
    case when IS_NULLABLE = 'YES' then 1 else 0 end as IsNullable
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '{tableName}';";
            Query(sql, out columnInfo);
            return this;
        }

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out DataTable dtResult)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection GetTriggers(string tableName, out DataTable dtResult)
        {
            throw new System.NotImplementedException();
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string sqlDefinition)
        {
            throw new System.NotImplementedException();
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
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            dtResult = new DataTable();
            bool newConnectionUsed = true;
            SqlConnection localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqlConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                using (SqlCommand command = new SqlCommand(sqlRequest, localConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dtResult = GetDataTable(reader);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                    localConnection = null;
                }
            }
            return this;
        }

        public IVelocipedeDbConnection Execute(string sqlRequest)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqlConnection localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqlConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                localConnection.Execute(sqlRequest);
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                    localConnection = null;
                }
            }
            return this;
        }

        public IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqlConnection localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqlConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                result = localConnection.Query<T>(sqlRequest).ToList();
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
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
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            SqlConnection localConnection = null;
            try
            {
                // Initialize connection.
                if (_connection != null)
                {
                    newConnectionUsed = false;
                    localConnection = _connection;
                }
                else
                {
                    localConnection = new SqlConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                result = localConnection.QueryFirstOrDefault<T>(sqlRequest);
            }
            catch (ArgumentException ex)
            {
                throw new VelocipedeConnectionStringException(ex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (newConnectionUsed && localConnection != null)
                {
                    localConnection.Close();
                    localConnection.Dispose();
                    localConnection = null;
                }
            }
            return this;
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

        /// <summary>
        /// Get database name by connection string.
        /// </summary>
        public static string GetDatabaseName(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                return connectionStringBuilder.InitialCatalog;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        /// <summary>
        /// Get connection string by database name.
        /// </summary>
        public static string GetConnectionString(string connectionString, string databaseName)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                connectionStringBuilder.InitialCatalog = databaseName;
                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        /// <summary>
        /// Get connection string by database name.
        /// </summary>
        private string UsePersistSecurityInfo(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                connectionStringBuilder.PersistSecurityInfo = true;
                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        public void Dispose()
        {
            CloseDb();
        }
    }
}

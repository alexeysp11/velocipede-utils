using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// PostgreSQL database connection.
    /// </summary>
    public sealed class PgDbConnection : IVelocipedeDbConnection
    {
        private NpgsqlConnection _connection;

        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.PostgreSQL;
        public string DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        public PgDbConnection(string connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            if (_connection != null)
                return true;

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
            return true;
        }

        public IVelocipedeDbConnection CreateDb()
        {
            if (DbExists())
                throw new InvalidOperationException(ErrorMessageConstants.DatabaseAlreadyExists);

            try
            {
                string dbName = DatabaseName;
                throw new System.NotImplementedException();
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
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            try
            {
                CloseDb();
                _connection = new NpgsqlConnection(ConnectionString);
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
            string sql = "SELECT t.schemaname || '.' || t.relname AS name FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t";
            Query(sql, out tables);
            return this;
        }

        public IVelocipedeDbConnection GetColumns(string tableName, out DataTable dtResult)
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

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out DataTable dtResult)
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

        public IVelocipedeDbConnection GetTriggers(string tableName, out DataTable dtResult)
        {
            string sql = string.Format(@"SELECT * FROM information_schema.triggers WHERE event_object_table LIKE '{0}'", tableName);
            ExecuteSqlCommand(sql, out dtResult);
            return this;
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string sqlDefinition)
        {
            string[] tn = tableName.Split('.');
            string sql = string.Format(@"
CREATE OR REPLACE FUNCTION fGetSqlFromTable(aSchemaName VARCHAR(255), aTableName VARCHAR(255))
RETURNS TEXT
LANGUAGE plpgsql AS
$func$
DECLARE
i INTEGER;
lNumRec INTEGER;
rec RECORD;
lResult text;
BEGIN
i := 0;
SELECT COUNT(*) INTO lNumRec FROM information_schema.columns WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName;
lResult := 'CREATE TABLE ' || aSchemaName || '.' || aTableName || chr(10) || '(' || chr(10);
FOR rec IN (
    SELECT 
        column_name, 
        column_default, 
        is_nullable, 
        data_type, 
        character_maximum_length
    FROM information_schema.columns
    WHERE table_schema LIKE aSchemaName AND table_name LIKE aTableName
)
LOOP
    i := i + 1;
    lResult := lResult || '    ' || rec.column_name || ' ' || rec.data_type;
    IF UPPER(rec.data_type) LIKE '%CHAR%VAR%' THEN 
        lResult := lResult || '(' || rec.character_maximum_length || ')';
    END IF;
    IF rec.column_default IS NOT NULL AND rec.column_default <> '' THEN
        lResult := lResult || ' DEFAULT ' || rec.column_default;
    END IF;
    IF i = lNumRec THEN 
        lResult := lResult || chr(10) || ');';
    ELSE 
        lResult := lResult || ', ' || chr(10);
    END IF;
END LOOP;

RETURN lResult;
END
$func$;

SELECT fGetSqlFromTable('{0}', '{1}') AS sql;", tn[0], tn[1]);
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

        public IVelocipedeDbConnection Query<T>(string sqlRequest, out List<T> result)
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

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T result)
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

        /// <summary>
        /// Get database name by connection string.
        /// </summary>
        public static string GetDatabaseName(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                return connectionStringBuilder.Database;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        /// <summary>
        /// Get connection string by database name.
        /// </summary>
        public static string GetConnectionString(string databaseName)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
            connectionStringBuilder.Database = databaseName;
            return connectionStringBuilder.ConnectionString;
        }

        public void Dispose()
        {
            CloseDb();
        }
    }
}

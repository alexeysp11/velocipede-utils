using System.Data;
using Dapper;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// PostgreSQL database connection.
    /// </summary>
    public sealed class PgDbConnection : IVelocipedeDbConnection
    {
        private NpgsqlConnection? _connection;

        public string? ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.PostgreSQL;
        public string? DatabaseName => GetDatabaseName(ConnectionString);
        public bool IsConnected => _connection != null;

        public PgDbConnection(string? connectionString = null)
        {
            ConnectionString = connectionString;
        }

        public bool DbExists()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            if (_connection != null && _connection.ConnectionString == ConnectionString)
                return true;

            string? existingConnectionString = _connection?.ConnectionString;
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
        private void OpenDb(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            try
            {
                CloseDb();
                connectionString = UsePersistSecurityInfo(connectionString);
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();
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
        private bool TryReconnect(string? connectionString)
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
        public IVelocipedeDbConnection SwitchDb(string? dbName, out string connectionString)
        {
            if (string.IsNullOrEmpty(dbName))
                throw new VelocipedeDbNameException();

            try
            {
                // Change connection string.
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = ConnectionString;
                connectionStringBuilder.Database = dbName;
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
            catch (PostgresException ex)
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
            string sql = "SELECT t.schemaname || '.' || t.relname AS name FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t";
            Query(sql, out tables);
            return this;
        }

        public IVelocipedeDbConnection GetColumns(string tableName, out List<VelocipedeColumnInfo> columnInfo)
        {
            string schemaName = "public";
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = string.Format($@"
SELECT
    column_name as ColumnName,
    ordinal_position as OrdinalPosition,
    column_default as DefaultValue,
    case when is_nullable = 'YES' then true else false end as IsNullable,
    data_type as ColumnType,
    case when is_self_referencing = 'YES' then true else false end as IsSelfReferencing,
    case when is_generated = 'ALWAYS' then true else false end as IsGenerated,
    case when is_updatable = 'YES' then true else false end as IsUpdatable
FROM information_schema.columns
WHERE table_schema = '{schemaName}' AND table_name = '{tableName}'");
            Query(sql, out columnInfo);
            return this;
        }

        public IVelocipedeDbConnection GetForeignKeys(string tableName, out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
        {
            string schemaName = "public";
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = string.Format(@"
SELECT
    tc.constraint_name as ConstraintName,
    tc.table_schema as FromTableSchema,
    tc.table_name as FromTableName,
    kcu.column_name as FromColumn,
    ccu.table_schema AS ToTableSchema,
    ccu.table_name AS ToTableName,
    ccu.column_name AS ToColumn
FROM 
    information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name AND ccu.table_schema = tc.table_schema
WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_name LIKE '{0}';", tableName);
            Query(sql, out foreignKeyInfo);
            return this;
        }

        public IVelocipedeDbConnection GetTriggers(string tableName, out List<VelocipedeTriggerInfo> triggerInfo)
        {
            string schemaName = "public";
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = string.Format(@"
SELECT
    trigger_catalog AS TriggerCatalog,
    trigger_schema AS TriggerSchema,
    trigger_name AS TriggerName,
    event_manipulation AS EventManipulation,
    event_object_catalog AS EventObjectCatalog,
    event_object_schema AS EventObjectSchema,
    event_object_table AS EventObjectTable,
    action_order AS ActionOrder,
    action_condition AS ActionCondition,
    action_statement AS ActionStatement,
    action_orientation AS ActionOrientation,
    action_timing AS ActionTiming,
    action_reference_old_table AS ActionReferenceOldTable,
    action_reference_new_table AS ActionReferenceNewTable
FROM information_schema.triggers
WHERE event_object_table LIKE '{0}'", tableName);
            Query(sql, out triggerInfo);
            return this;
        }

        public IVelocipedeDbConnection GetSqlDefinition(string tableName, out string? sqlDefinition)
        {
            string schemaName = "public";
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

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

SELECT fGetSqlFromTable('{0}', '{1}') AS sql;", schemaName, tableName);
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
            return ExecuteSqlCommand(
                sqlRequest,
                parameters: null,
                dtResult: out dtResult);
        }

        public IVelocipedeDbConnection ExecuteSqlCommand(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out DataTable dtResult)
        {
            return ExecuteSqlCommand(
                sqlRequest,
                parameters: null,
                predicate: null,
                dtResult: out dtResult);
        }

        public IVelocipedeDbConnection ExecuteSqlCommand(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<dynamic, bool>? predicate,
            out DataTable dtResult)
        {
            Query(sqlRequest, parameters, predicate, out List<dynamic> dynamicList);
            dtResult = dynamicList.ToDataTable();
            return this;
        }

        public IVelocipedeDbConnection Execute(string sqlRequest)
        {
            return Execute(sqlRequest, null);
        }

        public IVelocipedeDbConnection Execute(string sqlRequest, List<VelocipedeCommandParameter>? parameters)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            NpgsqlConnection? localConnection = null;
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
                    localConnection = new NpgsqlConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                localConnection.Execute(sqlRequest, parameters?.ToDapperParameters());
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
            return Query(
                sqlRequest,
                parameters: null,
                result: out result);
        }

        public IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out List<T> result)
        {
            return Query(
                sqlRequest,
                parameters: null,
                predicate: null,
                result: out result);
        }

        public IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out List<T> result)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            NpgsqlConnection? localConnection = null;
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
                    localConnection = new NpgsqlConnection(ConnectionString);
                }
                if (localConnection.State != ConnectionState.Open)
                {
                    localConnection.Open();
                }

                // Execute SQL command and dispose connection if necessary.
                IEnumerable<T> queryResult = localConnection.Query<T>(sqlRequest, parameters?.ToDapperParameters());
                if (predicate != null)
                    queryResult = queryResult.Where(predicate);
                result = queryResult.ToList();
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

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(string sqlRequest, out T? result)
        {
            return QueryFirstOrDefault(
                sqlRequest,
                parameters: null,
                result: out result);
        }

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out T? result)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

            bool newConnectionUsed = true;
            NpgsqlConnection? localConnection = null;
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
                    localConnection = new NpgsqlConnection(ConnectionString);
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

        public IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<T, bool>? predicate,
            out T? result)
        {
            if (predicate != null)
            {
                Query(sqlRequest, parameters, out List<T> list);
                result = list.FirstOrDefault(predicate);
                return this;
            }
            return QueryFirstOrDefault(sqlRequest, parameters, out result);
        }

        /// <summary>
        /// Get database name by connection string.
        /// </summary>
        public static string? GetDatabaseName(string? connectionString)
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
        public static string GetConnectionString(string? connectionString, string databaseName)
        {
            try
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = connectionString;
                connectionStringBuilder.Database = databaseName;
                connectionStringBuilder.PersistSecurityInfo = true;
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
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
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

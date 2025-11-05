using System.Data;
using Dapper;
using Npgsql;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Iterators;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// PostgreSQL database connection.
    /// </summary>
    public sealed class PgDbConnection : IVelocipedeDbConnection
    {
        private NpgsqlConnection? _connection;

        /// <inheritdoc/>
        public string? ConnectionString { get; set; }

        /// <inheritdoc/>
        public DatabaseType DatabaseType => DatabaseType.PostgreSQL;

        /// <inheritdoc/>
        public string? DatabaseName => GetDatabaseName(ConnectionString);

        /// <inheritdoc/>
        public bool IsConnected => _connection != null;

        public PgDbConnection(string? connectionString = null)
        {
            ConnectionString = connectionString;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IVelocipedeDbConnection OpenDb()
        {
            OpenDb(ConnectionString);
            return this;
        }

        /// <summary>
        /// Open database with the specified connection string.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
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
        /// <param name="connectionString">Connection string.</param>
        /// <returns><c>true</c> if connected successfully; otherwise, <c>false</c>.</returns>
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

        /// <inheritdoc/>
        public IVelocipedeDbConnection SwitchDb(
            string? dbName,
            out string connectionString)
        {
            if (string.IsNullOrEmpty(dbName))
                throw new VelocipedeDbNameException();

            try
            {
                // Change connection string.
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    ConnectionString = ConnectionString ?? "",
                    Database = dbName
                };
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetTablesInDb(out List<string> tables)
        {
            string sql = "SELECT t.schemaname || '.' || t.relname AS name FROM (SELECT schemaname, relname FROM pg_stat_user_tables) t";
            Query(sql, out tables);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetColumns(
            string tableName,
            out List<VelocipedeColumnInfo> columnInfo)
        {
            string schemaName = "public";
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = @"
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
WHERE table_schema = @SchemaName AND table_name = @TableName";
            List<VelocipedeCommandParameter> parameters =
            [
                new() { Name = "TableName", Value = tableName },
                new() { Name = "SchemaName", Value = schemaName }
            ];
            Query(sql, parameters, out columnInfo);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetForeignKeys(
            string tableName,
            out List<VelocipedeForeignKeyInfo> foreignKeyInfo)
        {
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                string schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = @"
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
WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_name = @TableName";
            List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
            Query(sql, parameters, out foreignKeyInfo);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetTriggers(
            string tableName,
            out List<VelocipedeTriggerInfo> triggerInfo)
        {
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                string schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = @"
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
WHERE event_object_table = @TableName";
            List<VelocipedeCommandParameter> parameters = [new() { Name = "TableName", Value = tableName }];
            Query(sql, parameters, out triggerInfo);
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection GetSqlDefinition(
            string tableName,
            out string? sqlDefinition)
        {
            string schemaName = "public";
            string[] tn = tableName.Split('.');
            if (tn.Length >= 2)
            {
                schemaName = tn.First();
                tableName = tableName.Replace($"{schemaName}.", "");
            }
            tableName = tableName.Trim('"');

            string sql = @"
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

SELECT fGetSqlFromTable(@SchemaName, @TableName) AS sql;";
            List<VelocipedeCommandParameter> parameters =
            [
                new() { Name = "TableName", Value = tableName },
                new() { Name = "SchemaName", Value = schemaName }
            ];
            return QueryFirstOrDefault(sql, parameters, out sqlDefinition);
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection CreateTemporaryTable(string tableName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection ClearTemporaryTable(string tableName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            out DataTable dtResult)
        {
            return QueryDataTable(
                sqlRequest,
                parameters: null,
                dtResult: out dtResult);
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out DataTable dtResult)
        {
            return QueryDataTable(
                sqlRequest,
                parameters,
                predicate: null,
                dtResult: out dtResult);
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection QueryDataTable(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            Func<dynamic, bool>? predicate,
            out DataTable dtResult)
        {
            Query(sqlRequest, parameters, predicate, out List<dynamic> dynamicList);
            dtResult = dynamicList.ToDataTable();
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection Execute(string sqlRequest)
        {
            return Execute(sqlRequest, null);
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection Execute(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters)
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
                }
            }
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            out List<T> result)
        {
            return Query(
                sqlRequest,
                parameters: null,
                result: out result);
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection Query<T>(
            string sqlRequest,
            List<VelocipedeCommandParameter>? parameters,
            out List<T> result)
        {
            return Query(
                sqlRequest,
                parameters,
                predicate: null,
                result: out result);
        }

        /// <inheritdoc/>
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
                }
            }
            return this;
        }

        /// <inheritdoc/>
        public IVelocipedeDbConnection QueryFirstOrDefault<T>(
            string sqlRequest,
            out T? result)
        {
            return QueryFirstOrDefault(
                sqlRequest,
                parameters: null,
                result: out result);
        }

        /// <inheritdoc/>
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
                result = localConnection.QueryFirstOrDefault<T>(sqlRequest, parameters?.ToDapperParameters());
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
                }
            }
            return this;
        }

        /// <inheritdoc/>
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
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Database name.</returns>
        public static string? GetDatabaseName(string? connectionString)
        {
            try
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    ConnectionString = connectionString
                };
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
        /// <param name="connectionString">Old connection string.</param>
        /// <param name="databaseName">Database name.</param>
        /// <returns>New connection string.</returns>
        public static string GetConnectionString(string? connectionString, string databaseName)
        {
            try
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    ConnectionString = connectionString ?? "",
                    Database = databaseName,
                    PersistSecurityInfo = true
                };
                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        /// <summary>
        /// Get connection string adding <see cref="NpgsqlConnectionStringBuilder.PersistSecurityInfo"/>.
        /// </summary>
        /// <param name="connectionString">Old connection string.</param>
        /// <returns>New connection string.</returns>
        private static string UsePersistSecurityInfo(string connectionString)
        {
            try
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    ConnectionString = connectionString,
                    PersistSecurityInfo = true
                };
                return connectionStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new VelocipedeDbNameException(ex);
            }
        }

        /// <inheritdoc/>
        public IVelocipedeForeachTableIterator WithForeachTableIterator(List<string> tables)
        {
            return new VelocipedeForeachTableIterator(this, tables);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            CloseDb();
        }
    }
}

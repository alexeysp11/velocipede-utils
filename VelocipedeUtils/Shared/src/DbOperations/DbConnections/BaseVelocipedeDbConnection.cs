using Dapper;
using System.Data;
using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections;

/// <summary>
/// Base class for implementing <see cref="IVelocipedeDbConnection"/>.
/// </summary>
public abstract class BaseVelocipedeDbConnection
{
    /// <summary>
    /// Internal method for executing SQL query.
    /// </summary>
    /// <param name="connection">Current instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="sqlRequest">SQL query to execute.</param>
    /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
    protected static void InternalExecute(
        IVelocipedeDbConnection connection,
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        bool newConnectionUsed = true;
        DbConnection? localConnection = null;
        try
        {
            // Initialize connection.
            if (connection.Connection != null)
            {
                newConnectionUsed = false;
                localConnection = connection.Connection;
            }
            else
            {
                localConnection = connection.CreateConnection(connection.ConnectionString);
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
        finally
        {
            if (newConnectionUsed && localConnection != null)
            {
                localConnection.Close();
                localConnection.Dispose();
            }
        }
    }

    /// <summary>
    /// Internal method for asynchronously executing SQL query.
    /// </summary>
    /// <param name="connection">Current instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="sqlRequest">SQL query.</param>
    /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected static async Task InternalExecuteAsync(
        IVelocipedeDbConnection connection,
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        bool newConnectionUsed = true;
        Task<DynamicParameters?>? dapperParamsTask = parameters?.ToDapperParametersAsync();
        DbConnection? localConnection = null;
        try
        {
            // Initialize connection.
            if (connection.Connection != null)
            {
                newConnectionUsed = false;
                localConnection = connection.Connection;
            }
            else
            {
                localConnection = connection.CreateConnection(connection.ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                await localConnection.OpenAsync();
            }

            // Execute SQL command and dispose connection if necessary.
            DynamicParameters? dynamicParameters = dapperParamsTask == null ? null : await dapperParamsTask;
            await localConnection.ExecuteAsync(sqlRequest, dynamicParameters);
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
        finally
        {
            if (newConnectionUsed && localConnection != null)
            {
                await localConnection.CloseAsync();
                localConnection.Dispose();
            }
        }
    }

    /// <summary>
    /// Internal method for query to get <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
    /// <param name="connection">Current instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="sqlRequest">SQL query.</param>
    /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
    /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
    /// <returns><see cref="List{T}"/> that contains the result of the executed query.</returns>
    protected static List<T> InternalQuery<T>(
        IVelocipedeDbConnection connection,
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        bool newConnectionUsed = true;
        DbConnection? localConnection = null;
        try
        {
            // Initialize connection.
            if (connection.Connection != null)
            {
                newConnectionUsed = false;
                localConnection = connection.Connection;
            }
            else
            {
                localConnection = connection.CreateConnection(connection.ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                localConnection.Open();
            }

            // Execute SQL command and dispose connection if necessary.
            IEnumerable<T> queryResult = localConnection.Query<T>(sqlRequest, parameters?.ToDapperParameters());
            if (predicate != null)
                queryResult = queryResult.Where(predicate);
            return queryResult.ToList();
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
        finally
        {
            if (newConnectionUsed && localConnection != null)
            {
                localConnection.Close();
                localConnection.Dispose();
            }
        }
    }

    /// <summary>
    /// Internal method for asynchronous query to get <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
    /// <param name="connection">Current instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="sqlRequest">SQL query.</param>
    /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
    /// <param name="predicate">Predicate that is executed strictly after the data has already been retrieved from the database.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result is a <see cref="List{T}"/> that contains the result of the executed query.
    /// </returns>
    protected static async Task<List<T>> InternalQueryAsync<T>(
        IVelocipedeDbConnection connection,
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters,
        Func<T, bool>? predicate)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        bool newConnectionUsed = true;
        Task<DynamicParameters?>? dapperParamsTask = parameters?.ToDapperParametersAsync();
        DbConnection? localConnection = null;
        try
        {
            // Initialize connection.
            if (connection.Connection != null)
            {
                newConnectionUsed = false;
                localConnection = connection.Connection;
            }
            else
            {
                localConnection = connection.CreateConnection(connection.ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                await localConnection.OpenAsync();
            }

            // Execute SQL command and dispose connection if necessary.
            DynamicParameters? dynamicParameters = dapperParamsTask == null ? null : await dapperParamsTask;
            IEnumerable<T> queryResult = await localConnection.QueryAsync<T>(sqlRequest, dynamicParameters);
            if (predicate != null)
                queryResult = queryResult.Where(predicate);
            return queryResult.ToList();
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
        finally
        {
            if (newConnectionUsed && localConnection != null)
            {
                await localConnection.CloseAsync();
                localConnection.Dispose();
            }
        }
    }

    /// <summary>
    /// Internal method for query first or default object.
    /// </summary>
    /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
    /// <param name="connection">Current instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="sqlRequest">SQL query.</param>
    /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
    /// <returns>Result of the query.</returns>
    protected static T? InternalQueryFirstOrDefault<T>(
        IVelocipedeDbConnection connection,
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        bool newConnectionUsed = true;
        DbConnection? localConnection = null;
        try
        {
            // Initialize connection.
            if (connection.Connection != null)
            {
                newConnectionUsed = false;
                localConnection = connection.Connection;
            }
            else
            {
                localConnection = connection.CreateConnection(connection.ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                localConnection.Open();
            }

            // Execute SQL command and dispose connection if necessary.
            return localConnection.QueryFirstOrDefault<T>(sqlRequest, parameters?.ToDapperParameters());
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
        finally
        {
            if (newConnectionUsed && localConnection != null)
            {
                localConnection.Close();
                localConnection.Dispose();
            }
        }
    }

    /// <summary>
    /// Internal method for asynchronous query first or default object.
    /// </summary>
    /// <typeparam name="T">The data type to which the query result is converted.</typeparam>
    /// <param name="connection">Current instance of <see cref="IVelocipedeDbConnection"/>.</param>
    /// <param name="sqlRequest">SQL query.</param>
    /// <param name="parameters"><see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> that contains query parameters.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation that contains the result of the executed query.
    /// </returns>
    protected static async Task<T?> InternalQueryFirstOrDefaultAsync<T>(
        IVelocipedeDbConnection connection,
        string sqlRequest,
        List<VelocipedeCommandParameter>? parameters)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new InvalidOperationException(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);

        bool newConnectionUsed = true;
        Task<DynamicParameters?>? dapperParamsTask = parameters?.ToDapperParametersAsync();
        DbConnection? localConnection = null;
        try
        {
            // Initialize connection.
            if (connection.Connection != null)
            {
                newConnectionUsed = false;
                localConnection = connection.Connection;
            }
            else
            {
                localConnection = connection.CreateConnection(connection.ConnectionString);
            }
            if (localConnection.State != ConnectionState.Open)
            {
                await localConnection.OpenAsync();
            }

            // Execute SQL command and dispose connection if necessary.
            DynamicParameters? dynamicParameters = dapperParamsTask == null ? null : await dapperParamsTask;
            return await localConnection.QueryFirstOrDefaultAsync<T>(sqlRequest, dynamicParameters);
        }
        catch (ArgumentException ex)
        {
            throw new VelocipedeConnectionStringException(ex);
        }
        finally
        {
            if (newConnectionUsed && localConnection != null)
            {
                await localConnection.CloseAsync();
                localConnection.Dispose();
            }
        }
    }
}

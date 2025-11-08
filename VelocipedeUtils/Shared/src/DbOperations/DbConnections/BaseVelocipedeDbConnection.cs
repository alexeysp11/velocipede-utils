using Dapper;
using System.Data;
using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
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
    }
}

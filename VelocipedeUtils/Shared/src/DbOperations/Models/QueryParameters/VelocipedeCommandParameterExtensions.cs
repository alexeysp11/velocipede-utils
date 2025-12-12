using Dapper;
using VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;

namespace VelocipedeUtils.Shared.DbOperations.Models.QueryParameters;

/// <summary>
/// Extensions for working with <see cref="VelocipedeCommandParameter"/> objects.
/// </summary>
public static class VelocipedeCommandParameterExtensions
{
    /// <summary>
    /// Convert a list of <see cref="VelocipedeCommandParameter"/> to Dapper's <see cref="DynamicParameters"/>.
    /// </summary>
    /// <param name="parameters">List of parameters.</param>
    /// <returns>If input <see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> is <c>null</c> or does not contain elements, then <c>null</c>; otherwise, a valid instance of Dapper's <see cref="DynamicParameters"/>.</returns>
    public static DynamicParameters? ToDapperParameters(this List<VelocipedeCommandParameter> parameters)
    {
        if (parameters == null || parameters.Count == 0)
            return null;

        var result = new DynamicParameters();
        foreach (var parameter in parameters)
        {
            result.Add(
                name: parameter.Name,
                value: parameter.Value,
                dbType: parameter.DbType,
                direction: parameter.Direction,
                size: parameter.Size,
                precision: parameter.Precision,
                scale: parameter.Scale);
        }
        return result;
    }

    /// <summary>
    /// Asynchronously convert a list of <see cref="VelocipedeCommandParameter"/> to Dapper's <see cref="DynamicParameters"/>.
    /// </summary>
    /// <param name="parameters">List of parameters.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation,
    /// the task result is Dapper's query parameters represented as <see cref="DynamicParameters"/>.
    /// If input <see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> is <c>null</c> or does not contain elements, then <c>null</c>; otherwise, a valid instance of Dapper's <see cref="DynamicParameters"/>.
    /// </returns>
    public static Task<DynamicParameters?> ToDapperParametersAsync(this List<VelocipedeCommandParameter> parameters)
    {
        return Task.Run(parameters.ToDapperParameters);
    }
}

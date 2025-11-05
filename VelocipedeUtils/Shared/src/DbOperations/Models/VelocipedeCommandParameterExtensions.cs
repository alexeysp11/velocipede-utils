using Dapper;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    /// <summary>
    /// Extensions for working with <see cref="VelocipedeCommandParameter"/> objects.
    /// </summary>
    public static class VelocipedeCommandParameterExtensions
    {
        /// <summary>
        /// Convert a list of <see cref="VelocipedeCommandParameter"/> to Dapper's <see cref="DynamicParameters"/>.
        /// </summary>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>If <see cref="List{T}"/> of <see cref="VelocipedeCommandParameter"/> is <c>null</c> or does not contain elements, then <c>null</c>; otherwise, a valid instance of Dapper's <see cref="DynamicParameters"/>.</returns>
        public static DynamicParameters? ToDapperParameters(this List<VelocipedeCommandParameter> parameters)
        {
            if (parameters == null || !parameters.Any())
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
    }
}

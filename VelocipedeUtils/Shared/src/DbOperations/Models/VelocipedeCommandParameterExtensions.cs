using Dapper;

namespace VelocipedeUtils.Shared.DbOperations.Models
{
    public static class VelocipedeCommandParameterExtensions
    {
        /// <summary>
        /// Convert a list of <see cref="VelocipedeCommandParameter"/> to Dapper's <see cref="DynamicParameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

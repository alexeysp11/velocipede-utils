using System.Linq.Expressions;

namespace VelocipedeUtils.Ubp.Core.Extensions;

public static class ExpressionExtensions
{
    /// <summary>
    /// Helper methods for building filter expressions
    /// </summary>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var parameter = expr1.Parameters[0];
        var body = Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}